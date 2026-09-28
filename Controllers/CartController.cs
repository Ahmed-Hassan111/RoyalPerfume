using Microsoft.AspNetCore.Mvc;
using RoyalPerfume.Services;

namespace RoyalPerfume.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;

        public CartController(CartService cartService, ProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            ViewBag.CartCount = cart.Items.Sum(i => i.Quantity);
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _productService.GetById(productId);
            if (product == null) return NotFound();

            _cartService.AddToCart(product, quantity);

            if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            {
                return Json(new { success = true, cartCount = _cartService.GetCartCount() });
            }
            TempData["Success"] = $"تم إضافة {product.NameAr} للسلة!";
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            _cartService.RemoveFromCart(productId);
            TempData["Success"] = "تم حذف المنتج من السلة";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            _cartService.UpdateQuantity(productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ApplyCoupon(string couponCode)
        {
            if (_cartService.ApplyCoupon(couponCode))
                TempData["CouponSuccess"] = "✅ تم تطبيق الكوبون بنجاح!";
            else
                TempData["CouponError"] = "❌ كود الكوبون غير صحيح";
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetCount()
        {
            return Json(new { count = _cartService.GetCartCount() });
        }
    }
}
