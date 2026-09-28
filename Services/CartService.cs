using Newtonsoft.Json;
using RoyalPerfume.Models;

namespace RoyalPerfume.Services
{
    public class CartService
    {
        private const string CartSessionKey = "ShoppingCart";
        private const string CartCookieKey  = "RoyalCart";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;
        private HttpRequest Request => _httpContextAccessor.HttpContext!.Request;
        private HttpResponse Response => _httpContextAccessor.HttpContext!.Response;

        public CartViewModel GetCart()
        {
            // 1. Try Session first (fast)
            var sessionJson = Session.GetString(CartSessionKey);
            if (!string.IsNullOrEmpty(sessionJson))
            {
                return JsonConvert.DeserializeObject<CartViewModel>(sessionJson) ?? new CartViewModel();
            }

            // 2. Fallback to Cookie (persists across browser restarts)
            var cookieJson = Request.Cookies[CartCookieKey];
            if (!string.IsNullOrEmpty(cookieJson))
            {
                var cart = JsonConvert.DeserializeObject<CartViewModel>(cookieJson) ?? new CartViewModel();
                // Restore into session
                Session.SetString(CartSessionKey, cookieJson);
                return cart;
            }

            return new CartViewModel();
        }

        public void SaveCart(CartViewModel cart)
        {
            var json = JsonConvert.SerializeObject(cart);
            // Save to both Session and Cookie
            Session.SetString(CartSessionKey, json);
            Response.Cookies.Append(CartCookieKey, json, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            });
        }

        public void AddToCart(Product product, int quantity = 1)
        {
            var cart = GetCart();
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.NameAr,
                    ProductImage = product.ImageUrl,
                    Price = product.Price,
                    OldPrice = product.OldPrice,
                    Quantity = quantity,
                    Volume = product.Volume
                });
            }
            SaveCart(cart);
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            cart.Items.RemoveAll(i => i.ProductId == productId);
            SaveCart(cart);
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                    cart.Items.Remove(item);
                else
                    item.Quantity = quantity;
            }
            SaveCart(cart);
        }

        public bool ApplyCoupon(string couponCode)
        {
            var cart = GetCart();
            // Valid coupons
            var coupons = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
            {
                { "ROYAL10", 0.10m },
                { "WELCOME20", 0.20m },
                { "VIP15", 0.15m }
            };

            if (coupons.TryGetValue(couponCode, out decimal discountRate))
            {
                cart.CouponCode = couponCode.ToUpper();
                cart.Discount = Math.Round(cart.Subtotal * discountRate);
                SaveCart(cart);
                return true;
            }
            return false;
        }

        public void ClearCart()
        {
            Session.Remove(CartSessionKey);
            // Delete the persistent cookie
            Response.Cookies.Delete(CartCookieKey);
        }

        public int GetCartCount()
        {
            var cart = GetCart();
            return cart.Items.Sum(i => i.Quantity);
        }
    }
}
