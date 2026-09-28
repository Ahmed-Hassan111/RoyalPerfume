using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RoyalPerfume.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.AddColumn<string>(
                name: "AlternatePhone",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlternatePhone",
                table: "Products");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Concentration", "Description", "DescriptionAr", "ImageUrl", "IsFeatured", "IsOnSale", "Name", "NameAr", "OldPrice", "Price", "Rating", "ReviewCount", "Stock", "Volume" },
                values: new object[,]
                {
                    { 1, "عطور رجالية", "Extrait 35%", "A masterpiece of French perfumery, a 35% Extrait concentration", "روائح طازجة تتمازج مع توابل أندلسية فاخرة تمنحك اتزاناً ونضارة طوال اليوم", "/images/product1.jpg", true, true, "Sapphire Al-Elixir Original", "سوفاج الإكسير i-Original", 8400m, 680m, 4.9000000000000004, 328, 15, "100 جم" },
                    { 2, "عطور نسائية", "Extrait 40%", "A 70ml floral and amber scent with double enhancement", "عطر فلوري عنبري بتعزيز مضاعف يمنحك جاذبية لا تُقاوم طوال اليوم", "/images/product2.jpg", true, true, "Baccarat Rouge 540 mi-Original", "بكارات روج 540 mi-Original", 12200m, 750m, 4.7999999999999998, 512, 8, "70 جم" },
                    { 3, "عود وعربية", "Pure Perfume", "A royal blend of authentic aged oud with rare oriental spices", "مزيج ملكي من العود الأصيل المعتق مع أفخر التوابل الشرقية النادرة", "/images/product3.jpg", true, true, "Oud Al-Maliki Royal", "عود الملكي رويال", 1500m, 950m, 5.0, 198, 5, "100 جم" },
                    { 4, "عطور نسائية", "EDP", "Tom Ford's iconic dark floral with rich truffle and orchid", "التوقيع الأيقوني من توم فورد بمزيج الكمأة الداكنة والأوركيدة الفاخرة", "/images/product4.jpg", false, true, "Black Orchid Original", "بلاك أوركيد الأصلي", 11000m, 820m, 4.7000000000000002, 445, 12, "100 جم" },
                    { 5, "عطور رجالية", "Extrait 35%", "Creed's legendary fragrance with birch, pineapple and ambergris", "العطر الأسطوري من كريد بمزيج البتولا والأناناس والعنبر الملكي", "/images/product5.jpg", true, true, "Aventus i-Original", "أفنتوس i-Original", 13500m, 890m, 4.9000000000000004, 623, 7, "100 جم" },
                    { 6, "عود وعربية", "Extrait", "Pure Taif rose essence combined with aged Hindi oud", "خلاصة وردة الطائف النقية تلتقي مع العود الهندي المعتق", "/images/product6.jpg", false, false, "Rose Oud Taifi", "ورد العود الطائفي", 980m, 680m, 4.5999999999999996, 87, 20, "50 جم" }
                });
        }
    }
}
