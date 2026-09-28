namespace RoyalPerfume.Models
{
    /// <summary>
    /// عنصر واحد داخل الطلب — يُحفظ في جدول OrderItems في قاعدة البيانات
    /// </summary>
    public class OrderItem
    {
        public int Id { get; set; }

        // FK → Orders
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int Quantity { get; set; }
        public string Volume { get; set; } = string.Empty;

        public decimal TotalPrice => Price * Quantity;
    }
}
