using Microsoft.EntityFrameworkCore;
using RoyalPerfume.Data;
using RoyalPerfume.Models;

namespace RoyalPerfume.Services
{
    /// <summary>
    /// خدمة الطلبات — تستخدم SQL Server عبر Entity Framework Core
    /// </summary>
    public class OrderService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<OrderService> _logger;

        public OrderService(AppDbContext db, ILogger<OrderService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public void AddOrder(Order order)
        {
            _db.Orders.Add(order);
            _db.SaveChanges();   // ← الحفظ الفعلي في SQL Server
            _logger.LogInformation("✅ Order {OrderNumber} saved to DB. Id={Id}", order.OrderNumber, order.Id);
        }

        public List<Order> GetAll() =>
            _db.Orders
               .Include(o => o.Items)
               .OrderByDescending(o => o.OrderDate)
               .ToList();

        public Order? GetByNumber(string orderNumber) =>
            _db.Orders
               .Include(o => o.Items)
               .FirstOrDefault(o => o.OrderNumber == orderNumber);

        public int GetTotalOrders() => _db.Orders.Count();

        public decimal GetTotalRevenue() => _db.Orders.Sum(o => (decimal?)o.Total) ?? 0;

        public int GetInstapayOrders() =>
            _db.Orders.Count(o => o.PaymentMethod == "instapay");

        public bool DeleteOrder(string orderNumber)
        {
            var order = _db.Orders
                           .Include(o => o.Items)
                           .FirstOrDefault(o => o.OrderNumber == orderNumber);
            if (order == null) return false;

            _db.Orders.Remove(order);   // cascade deletes Items (FK with DeleteBehavior.Cascade)
            _db.SaveChanges();
            _logger.LogInformation("🗑️ Order {OrderNumber} deleted from DB.", orderNumber);
            return true;
        }

        public void UpdateStatus(string orderNumber, string status)
        {
            var order = _db.Orders.FirstOrDefault(o => o.OrderNumber == orderNumber);
            if (order != null)
            {
                order.Status = status;
                _db.SaveChanges();
                _logger.LogInformation("Order {OrderNumber} status → {Status}", orderNumber, status);
            }
        }
    }
}
