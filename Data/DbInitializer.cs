using RoyalPerfume.Models;

namespace RoyalPerfume.Data
{
    /// <summary>
    /// يُشغَّل مرة واحدة عند بدء التطبيق.
    /// يُدرج المنتجات الأساسية فقط إذا كان جدول Products فارغاً.
    /// بعد ذلك يستطيع الأدمن الإضافة/التعديل/الحذف بحرية كاملة.
    /// </summary>
    public static class DbInitializer
    {
        public static void Seed(AppDbContext db)
        {
            // لو في منتجات موجودة — لا تعمل حاجة
            if (db.Products.Any()) return;

            var products = new List<Product>
            {
                new()
                {
                    Name = "Sapphire Al-Elixir Original",
                    NameAr = "سوفاج الإكسير i-Original",
                    Description = "A masterpiece of French perfumery, a 35% Extrait concentration",
                    DescriptionAr = "روائح طازجة تتمازج مع توابل أندلسية فاخرة تمنحك اتزاناً ونضارة طوال اليوم",
                    Price = 680, OldPrice = 8400,
                    ImageUrl = "/images/product1.jpg", Category = "عطور رجالية",
                    Volume = "100 جم", Concentration = "Extrait 35%",
                    IsFeatured = true, IsOnSale = true, Stock = 15,
                    Rating = 4.9, ReviewCount = 328
                },
                new()
                {
                    Name = "Baccarat Rouge 540 mi-Original",
                    NameAr = "بكارات روج 540 mi-Original",
                    Description = "A 70ml floral and amber scent with double enhancement",
                    DescriptionAr = "عطر فلوري عنبري بتعزيز مضاعف يمنحك جاذبية لا تُقاوم طوال اليوم",
                    Price = 750, OldPrice = 12200,
                    ImageUrl = "/images/product2.jpg", Category = "عطور نسائية",
                    Volume = "70 جم", Concentration = "Extrait 40%",
                    IsFeatured = true, IsOnSale = true, Stock = 8,
                    Rating = 4.8, ReviewCount = 512
                },
                new()
                {
                    Name = "Oud Al-Maliki Royal",
                    NameAr = "عود الملكي رويال",
                    Description = "A royal blend of authentic aged oud with rare oriental spices",
                    DescriptionAr = "مزيج ملكي من العود الأصيل المعتق مع أفخر التوابل الشرقية النادرة",
                    Price = 950, OldPrice = 1500,
                    ImageUrl = "/images/product3.jpg", Category = "عود وعربية",
                    Volume = "100 جم", Concentration = "Pure Perfume",
                    IsFeatured = true, IsOnSale = true, Stock = 5,
                    Rating = 5.0, ReviewCount = 198
                },
                new()
                {
                    Name = "Black Orchid Original",
                    NameAr = "بلاك أوركيد الأصلي",
                    Description = "Tom Ford's iconic dark floral with rich truffle and orchid",
                    DescriptionAr = "التوقيع الأيقوني من توم فورد بمزيج الكمأة الداكنة والأوركيدة الفاخرة",
                    Price = 820, OldPrice = 11000,
                    ImageUrl = "/images/product4.jpg", Category = "عطور نسائية",
                    Volume = "100 جم", Concentration = "EDP",
                    IsFeatured = false, IsOnSale = true, Stock = 12,
                    Rating = 4.7, ReviewCount = 445
                },
                new()
                {
                    Name = "Aventus i-Original",
                    NameAr = "أفنتوس i-Original",
                    Description = "Creed's legendary fragrance with birch, pineapple and ambergris",
                    DescriptionAr = "العطر الأسطوري من كريد بمزيج البتولا والأناناس والعنبر الملكي",
                    Price = 890, OldPrice = 13500,
                    ImageUrl = "/images/product5.jpg", Category = "عطور رجالية",
                    Volume = "100 جم", Concentration = "Extrait 35%",
                    IsFeatured = true, IsOnSale = true, Stock = 7,
                    Rating = 4.9, ReviewCount = 623
                },
                new()
                {
                    Name = "Rose Oud Taifi",
                    NameAr = "ورد العود الطائفي",
                    Description = "Pure Taif rose essence combined with aged Hindi oud",
                    DescriptionAr = "خلاصة وردة الطائف النقية تلتقي مع العود الهندي المعتق",
                    Price = 680, OldPrice = 980,
                    ImageUrl = "/images/product6.jpg", Category = "عود وعربية",
                    Volume = "50 جم", Concentration = "Extrait",
                    IsFeatured = false, IsOnSale = false, Stock = 20,
                    Rating = 4.6, ReviewCount = 87
                }
            };

            db.Products.AddRange(products);
            db.SaveChanges();
        }
    }
}
