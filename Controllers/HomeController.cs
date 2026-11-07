using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using RoyalPerfume.Models;
using RoyalPerfume.Services;

namespace RoyalPerfume.Controllers;

public class HomeController : Controller
{
    private readonly ProductService _productService;
    private readonly CartService _cartService;
    private readonly IConfiguration _config;

    public HomeController(ProductService productService, CartService cartService, IConfiguration config)
    {
        _productService = productService;
        _cartService = cartService;
        _config = config;
    }

    public IActionResult Index()
    {
        var featuredProducts = _productService.GetFeatured();
        var onSaleProducts = _productService.GetOnSale();
        ViewBag.FeaturedProducts = featuredProducts;
        ViewBag.OnSaleProducts = onSaleProducts;
        ViewBag.CartCount = _cartService.GetCartCount();
        return View();
    }

    public IActionResult ReturnPolicy()
    {
        ViewBag.CartCount = _cartService.GetCartCount();
        return View();
    }

    // ── Contact Us ──────────────────────────────────────────────
    public IActionResult Contact()
    {
        ViewBag.CartCount = _cartService.GetCartCount();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(string name, string email, string message)
    {
        ViewBag.CartCount = _cartService.GetCartCount();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
        {
            TempData["Error"] = "يرجى ملء جميع الحقول المطلوبة.";
            return View();
        }

        try
        {
            var host     = _config["Email:Host"]     ?? "smtp.gmail.com";
            var port     = int.Parse(_config["Email:Port"] ?? "587");
            var from     = _config["Email:From"]     ?? "";
            var username = _config["Email:Username"] ?? "";
            var password = _config["Email:Password"] ?? "";

            using var smtp = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl   = true
            };

            var mail = new MailMessage
            {
                From       = new MailAddress(from, "أطياب رويال - تواصل معنا"),
                Subject    = $"رسالة جديدة من {name}",
                Body       = $"الاسم: {name}\nالبريد: {email}\n\nالرسالة:\n{message}",
                IsBodyHtml = false
            };
            mail.To.Add(from);
            mail.ReplyToList.Add(new MailAddress(email, name));

            await smtp.SendMailAsync(mail);
            TempData["Success"] = "تم إرسال رسالتك بنجاح! سنرد عليك في أقرب وقت.";
        }
        catch
        {
            TempData["Success"] = "تم استلام رسالتك! سنتواصل معك قريباً.";
        }

        return RedirectToAction(nameof(Contact));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
