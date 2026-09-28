using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RoyalPerfume.Models;
using RoyalPerfume.Services;

namespace RoyalPerfume.Controllers;

public class HomeController : Controller
{
    private readonly ProductService _productService;
    private readonly CartService _cartService;

    public HomeController(ProductService productService, CartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
