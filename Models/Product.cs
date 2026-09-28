namespace RoyalPerfume.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Volume { get; set; } = string.Empty;
        public string Concentration { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public bool IsOnSale { get; set; }
        public int Stock { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string AlternatePhone { get; set; } = string.Empty;

        public int DiscountPercent => OldPrice > 0 
            ? (int)Math.Round((1 - Price / OldPrice) * 100) 
            : 0;
    }
}
