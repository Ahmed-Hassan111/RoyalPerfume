using Microsoft.EntityFrameworkCore;
using RoyalPerfume.Data;
using RoyalPerfume.Models;

namespace RoyalPerfume.Services
{
    /// <summary>
    /// خدمة المنتجات — تستخدم SQL Server عبر Entity Framework Core
    /// </summary>
    public class ProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        public List<Product> GetAll() =>
            _db.Products.OrderBy(p => p.Id).ToList();

        public List<Product> GetFeatured() =>
            _db.Products.Where(p => p.IsFeatured).ToList();

        public List<Product> GetOnSale() =>
            _db.Products.Where(p => p.IsOnSale).ToList();

        public Product? GetById(int id) =>
            _db.Products.FirstOrDefault(p => p.Id == id);

        public List<Product> GetByCategory(string category) =>
            _db.Products.Where(p => p.Category == category).ToList();

        public List<string> GetCategories() =>
            _db.Products.Select(p => p.Category).Distinct().ToList();

        public void AddProduct(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
        }

        public bool UpdateProduct(Product updated)
        {
            var existing = _db.Products.FirstOrDefault(p => p.Id == updated.Id);
            if (existing == null) return false;

            existing.Name          = updated.Name;
            existing.NameAr        = updated.NameAr;
            existing.Description   = updated.Description;
            existing.DescriptionAr = updated.DescriptionAr;
            existing.Price         = updated.Price;
            existing.OldPrice      = updated.OldPrice;
            existing.ImageUrl      = updated.ImageUrl;
            existing.Category      = updated.Category;
            existing.Volume        = updated.Volume;
            existing.Concentration = updated.Concentration;
            existing.IsFeatured    = updated.IsFeatured;
            existing.IsOnSale      = updated.IsOnSale;
            existing.Stock         = updated.Stock;
            existing.Rating        = updated.Rating;
            existing.ReviewCount   = updated.ReviewCount;

            _db.SaveChanges();
            return true;
        }

        public bool DeleteProduct(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;
            _db.Products.Remove(product);
            _db.SaveChanges();
            return true;
        }

        public int GetTotalStock() => _db.Products.Sum(p => p.Stock);
        public int GetProductCount() => _db.Products.Count();
    }
}
