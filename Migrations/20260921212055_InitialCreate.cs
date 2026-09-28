using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RoyalPerfume.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AlternatePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CouponCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReceiptImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Volume = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Concentration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsOnSale = table.Column<bool>(type: "bit", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    ReviewCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProductImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Volume = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
