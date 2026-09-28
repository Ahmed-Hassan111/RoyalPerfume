namespace RoyalPerfume.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int Quantity { get; set; }
        public string Volume { get; set; } = string.Empty;

        public decimal TotalPrice => Price * Quantity;
    }

    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new();
        public string CouponCode { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public decimal Subtotal => Items.Sum(i => i.TotalPrice);
        public decimal ShippingCost { get; set; } = 0;
        public decimal Total => Subtotal - Discount + ShippingCost;
    }
}
