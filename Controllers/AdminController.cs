using Microsoft.AspNetCore.Mvc;
using RoyalPerfume.Models;
using RoyalPerfume.Services;

namespace RoyalPerfume.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ProductService productService, OrderService orderService,
            EmailService emailService, IConfiguration config, IWebHostEnvironment env,
            ILogger<AdminController> logger)
        {
            _productService = productService;
            _orderService = orderService;
            _emailService = emailService;
            _config = config;
            _env = env;
            _logger = logger;
        }

        // ==================== Authentication ====================

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string password)
        {
            var adminPass = _config["Admin:Password"] ?? "royal2024";
            if (password == adminPass)
            {
                HttpContext.Session.SetString("AdminLoggedIn", "true");
                return RedirectToAction("Index");
            }
            ViewBag.Error = "كلمة السر غير صحيحة";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminLoggedIn");
            return RedirectToAction("Login");
        }

        // ==================== Dashboard ====================

        public IActionResult Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");

            ViewBag.TotalOrders    = _orderService.GetTotalOrders();
            ViewBag.TotalRevenue   = _orderService.GetTotalRevenue();
            ViewBag.ProductCount   = _productService.GetProductCount();
            ViewBag.InstapayOrders = _orderService.GetInstapayOrders();
            ViewBag.Orders         = _orderService.GetAll();   // ALL orders — no .Take limit
            ViewBag.Products       = _productService.GetAll();
            return View();
        }

        // ==================== Products ====================

        public IActionResult Products()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            ViewBag.Products = _productService.GetAll();
            return View();
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            var product = _productService.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product model, IFormFile? NewImage)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");

            // Sanitize optional string fields — prevent SQL NOT NULL violation
            model.Description   = model.Description   ?? string.Empty;
            model.DescriptionAr = model.DescriptionAr ?? string.Empty;
            model.Volume        = model.Volume        ?? string.Empty;
            model.Concentration = model.Concentration ?? string.Empty;

            // Handle new image upload
            if (NewImage != null && NewImage.Length > 0)
            {
                var uploadsDir = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(uploadsDir);
                var ext = Path.GetExtension(NewImage.FileName);
                var fileName = $"product_{model.Id}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
                var filePath = Path.Combine(uploadsDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await NewImage.CopyToAsync(stream);
                }
                model.ImageUrl = $"/images/{fileName}";
            }

            _productService.UpdateProduct(model);
            TempData["Success"] = "تم تحديث المنتج بنجاح ✅";
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product model, IFormFile? NewImage)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");

            // Sanitize optional string fields — prevent SQL NOT NULL violation
            model.Description    = model.Description    ?? string.Empty;
            model.DescriptionAr  = model.DescriptionAr  ?? string.Empty;
            model.Volume         = model.Volume         ?? string.Empty;
            model.Concentration  = model.Concentration  ?? string.Empty;
            model.AlternatePhone = model.AlternatePhone ?? string.Empty;

            if (NewImage != null && NewImage.Length > 0)
            {
                var uploadsDir = Path.Combine(_env.WebRootPath, "images");
                var ext = Path.GetExtension(NewImage.FileName);
                var fileName = $"product_new_{DateTime.Now:yyyyMMddHHmmss}{ext}";
                var filePath = Path.Combine(uploadsDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await NewImage.CopyToAsync(stream);
                }
                model.ImageUrl = $"/images/{fileName}";
            }
            else
            {
                model.ImageUrl = "/images/product1.jpg"; // default
            }

            _productService.AddProduct(model);
            TempData["Success"] = "تمت إضافة المنتج بنجاح ✅";
            return RedirectToAction("Products");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            _productService.DeleteProduct(id);
            TempData["Success"] = "تم حذف المنتج ✅";
            return RedirectToAction("Products");
        }

        // ==================== Orders ====================

        public IActionResult Orders()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            ViewBag.Orders = _orderService.GetAll();
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]   // called via fetch — token verified by session check
        public IActionResult UpdateOrderStatus(string orderNumber, string status)
        {
            if (!IsLoggedIn()) return Json(new { success = false });
            _orderService.UpdateStatus(orderNumber, status);
            return Json(new { success = true });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult DeleteOrder(string orderNumber)
        {
            if (!IsLoggedIn()) return Json(new { success = false, message = "غير مصرح" });
            var deleted = _orderService.DeleteOrder(orderNumber);
            return Json(new { success = deleted, message = deleted ? "تم حذف الطلب ✅" : "الطلب غير موجود" });
        }

        // ==================== Diagnostics ====================

        public async Task<IActionResult> TestEmail()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            try
            {
                var testOrder = new Order
                {
                    OrderNumber = "TEST-" + DateTime.Now.ToString("HHmmss"),
                    OrderDate   = DateTime.Now,
                    FullName    = "اختبار الإيميل",
                    Phone       = "01000000000",
                    Governorate = "القاهرة",
                    Address     = "عنوان تجريبي",
                    PaymentMethod = "cash",
                    Total       = 999,
                    Subtotal    = 999,
                    Status      = "confirmed",
                    Items = new List<OrderItem>
                    {
                        new() { ProductName = "عطر تجريبي", Quantity = 1, Price = 999, Volume = "70 مل" }
                    }
                };
                await _emailService.SendOrderEmailAsync(testOrder);
                TempData["Success"] = "✅ تم إرسال إيميل الاختبار بنجاح! تحقق من بريدك الإلكتروني.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TestEmail failed");
                TempData["Success"] = $"❌ فشل إرسال الإيميل: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        public IActionResult DiagOrders()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            var orders = _orderService.GetAll();

            ViewBag.Orders          = orders;
            ViewBag.TotalOrders     = orders.Count;
            ViewBag.TotalRevenue    = orders.Sum(o => o.Total);
            ViewBag.TodayOrders     = orders.Count(o => o.OrderDate.Date == DateTime.Today);
            ViewBag.TodayRevenue    = orders.Where(o => o.OrderDate.Date == DateTime.Today).Sum(o => o.Total);
            ViewBag.CashOrders      = orders.Count(o => o.PaymentMethod == "cash");
            ViewBag.InstapayOrders  = orders.Count(o => o.PaymentMethod == "instapay");
            ViewBag.ConfirmedOrders = orders.Count(o => o.Status == "confirmed");
            ViewBag.PendingOrders   = orders.Count(o => o.Status == "pending_review" || o.Status == "pending");
            ViewBag.ShippedOrders   = orders.Count(o => o.Status == "shipped");
            ViewBag.DeliveredOrders = orders.Count(o => o.Status == "delivered");
            ViewBag.TopGov          = orders.GroupBy(o => o.Governorate)
                                            .OrderByDescending(g => g.Count())
                                            .Select(g => new { Name = g.Key, Count = g.Count() })
                                            .Take(5).ToList<dynamic>();
            return View();
        }

        private bool IsLoggedIn() =>
            HttpContext.Session.GetString("AdminLoggedIn") == "true";
    }
}
