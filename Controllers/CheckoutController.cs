using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoyalPerfume.Models;
using RoyalPerfume.Services;

namespace RoyalPerfume.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CartService _cartService;
        private readonly EmailService _emailService;
        private readonly OrderService _orderService;
        private readonly ILogger<CheckoutController> _logger;
        private static int _orderCounter = 1000;

        public CheckoutController(CartService cartService, EmailService emailService, OrderService orderService, ILogger<CheckoutController> logger)
        {
            _cartService = cartService;
            _emailService = emailService;
            _orderService = orderService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            if (!cart.Items.Any())
                return RedirectToAction("Index", "Cart");

            var model = new CheckoutViewModel { Cart = cart };
            ViewBag.CartCount = cart.Items.Sum(i => i.Quantity);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder([FromForm] CheckoutViewModel model)
        {
            var cart = _cartService.GetCart();
            if (!cart.Items.Any())
                return RedirectToAction("Index", "Cart");

            model.Cart = cart;

            // Force payment method to cash — clear any related ModelState errors
            model.PaymentMethod = "cash";
            ModelState.Remove(nameof(model.PaymentMethod));
            ModelState.Remove(nameof(model.ReceiptImage));
            // Optional fields — remove from validation so empty values don't block the order
            ModelState.Remove(nameof(model.Notes));
            ModelState.Remove(nameof(model.AlternatePhone));
            ModelState.Remove(nameof(model.District));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("PlaceOrder ModelState invalid: {Errors}",
                    string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)));
                ViewBag.CartCount = cart.Items.Sum(i => i.Quantity);
                return View("Index", model);
            }

            // Create order — deep-copy Items so ClearCart() doesn't erase them
            var orderNumber = $"ROY{DateTime.Now:yyyyMMdd}{Interlocked.Increment(ref _orderCounter)}";
            var order = new Order
            {
                OrderNumber = orderNumber,
                OrderDate = DateTime.Now,
                FullName = model.FullName,
                Phone = model.Phone,
                AlternatePhone = model.AlternatePhone ?? string.Empty,
                Address = model.Address,
                Governorate = model.Governorate,
                District = model.District ?? string.Empty,
                PaymentMethod = "cash",
                Notes = model.Notes ?? string.Empty,
                Items = cart.Items.Select(i => new RoyalPerfume.Models.OrderItem
                {
                    ProductId    = i.ProductId,
                    ProductName  = i.ProductName,
                    ProductImage = i.ProductImage,
                    Price        = i.Price,
                    OldPrice     = i.OldPrice,
                    Quantity     = i.Quantity,
                    Volume       = i.Volume
                }).ToList(),
                Subtotal = cart.Subtotal,
                Discount = cart.Discount,
                ShippingCost = cart.ShippingCost,
                Total = cart.Total,
                CouponCode = cart.CouponCode ?? string.Empty,
                ReceiptImagePath = string.Empty,
                Status = "confirmed"
            };

            // Save order — wrapped in try/catch to surface DB errors
            try
            {
                _orderService.AddOrder(order);
                _logger.LogInformation("✅ Order {OrderNumber} saved to DB. Id={Id}, Total orders now={Count}",
                    order.OrderNumber, order.Id, _orderService.GetTotalOrders());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ FAILED to save order {OrderNumber} to DB. FullName={Name}, Phone={Phone}",
                    orderNumber, model.FullName, model.Phone);
                // Show error back to user so they know it failed
                ModelState.AddModelError(string.Empty, $"خطأ في حفظ الطلب: {ex.InnerException?.Message ?? ex.Message}");
                ViewBag.CartCount = cart.Items.Sum(i => i.Quantity);
                ViewBag.DbError = ex.InnerException?.Message ?? ex.Message;
                return View("Index", model);
            }

            // Send email
            try
            {
                await _emailService.SendOrderEmailAsync(order);
                _logger.LogInformation("📧 Email sent for order {OrderNumber}", order.OrderNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send email for order {OrderNumber}", order.OrderNumber);
                // Don't block the user — continue even if email fails
            }

            // Clear cart
            _cartService.ClearCart();

            // Store order in TempData for success page
            TempData["OrderNumber"]  = order.OrderNumber;
            TempData["CustomerName"] = order.FullName;
            TempData["OrderTotal"]   = order.Total.ToString("N0");
            TempData["PaymentMethod"] = order.PaymentMethod;

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            ViewBag.OrderNumber = TempData["OrderNumber"];
            ViewBag.CustomerName = TempData["CustomerName"];
            ViewBag.OrderTotal = TempData["OrderTotal"];
            ViewBag.PaymentMethod = TempData["PaymentMethod"];
            ViewBag.CartCount = 0;
            return View();
        }
    }
}
