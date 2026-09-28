using Microsoft.EntityFrameworkCore;
using RoyalPerfume.Models;

namespace RoyalPerfume.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product>   Products   { get; set; }
        public DbSet<Order>     Orders     { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Order ────────────────────────────────────────────────────────
            modelBuilder.Entity<Order>(e =>
            {
                e.HasKey(o => o.Id);
                e.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);
                e.Property(o => o.FullName).IsRequired().HasMaxLength(200);
                e.Property(o => o.Phone).HasMaxLength(20);
                e.Property(o => o.AlternatePhone).HasMaxLength(20);
                e.Property(o => o.Governorate).HasMaxLength(100);
                e.Property(o => o.District).HasMaxLength(100);
                e.Property(o => o.Address).HasMaxLength(500);
                e.Property(o => o.PaymentMethod).HasMaxLength(50);
                e.Property(o => o.Status).HasMaxLength(50);
                e.Property(o => o.CouponCode).HasMaxLength(50);
                e.Property(o => o.Notes).HasMaxLength(1000);
                e.Property(o => o.ReceiptImagePath).HasMaxLength(500);
                e.Property(o => o.Subtotal).HasColumnType("decimal(18,2)");
                e.Property(o => o.Discount).HasColumnType("decimal(18,2)");
                e.Property(o => o.ShippingCost).HasColumnType("decimal(18,2)");
                e.Property(o => o.Total).HasColumnType("decimal(18,2)");

                e.HasMany(o => o.Items)
                 .WithOne(i => i.Order)
                 .HasForeignKey(i => i.OrderId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── OrderItem ────────────────────────────────────────────────────
            modelBuilder.Entity<OrderItem>(e =>
            {
                e.HasKey(i => i.Id);
                e.Property(i => i.ProductName).HasMaxLength(200);
                e.Property(i => i.ProductImage).HasMaxLength(500);
                e.Property(i => i.Volume).HasMaxLength(50);
                e.Property(i => i.Price).HasColumnType("decimal(18,2)");
                e.Property(i => i.OldPrice).HasColumnType("decimal(18,2)");
                e.Ignore(i => i.TotalPrice); // computed property — not stored
            });

            // ── Product ──────────────────────────────────────────────────────
            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Name).HasMaxLength(200);
                e.Property(p => p.NameAr).HasMaxLength(200);
                e.Property(p => p.Description).HasMaxLength(1000);
                e.Property(p => p.DescriptionAr).HasMaxLength(1000);
                e.Property(p => p.ImageUrl).HasMaxLength(500);
                e.Property(p => p.Category).HasMaxLength(100);
                e.Property(p => p.Volume).HasMaxLength(50);
                e.Property(p => p.Concentration).HasMaxLength(100);
                e.Property(p => p.Price).HasColumnType("decimal(18,2)");
                e.Property(p => p.OldPrice).HasColumnType("decimal(18,2)");
                e.Ignore(p => p.DiscountPercent); // computed property — not stored
            });
        }
    }
}
