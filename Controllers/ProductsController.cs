using Microsoft.AspNetCore.Mvc;
using RoyalPerfume.Services;

namespace RoyalPerfume.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;
        private readonly CartService _cartService;

        public ProductsController(ProductService productService, CartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public IActionResult Index(string? category = null)
        {
            var products = string.IsNullOrEmpty(category)
                ? _productService.GetAll()
                : _productService.GetByCategory(category);

            ViewBag.Categories = _productService.GetCategories();
            ViewBag.SelectedCategory = category;
            ViewBag.CartCount = _cartService.GetCartCount();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _productService.GetById(id);
            if (product == null) return NotFound();
            
            var relatedProducts = _productService.GetByCategory(product.Category)
                .Where(p => p.Id != id).Take(4).ToList();
            
            ViewBag.RelatedProducts = relatedProducts;
            ViewBag.CartCount = _cartService.GetCartCount();
            return View(product);
        }
    }
}
