using Microsoft.EntityFrameworkCore;
using RoyalPerfume.Data;
using RoyalPerfume.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ── SQL Server / EF Core ────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Session ─────────────────────────────────────────────────────────────────
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".RoyalPerfume.Session";
});

// ── HTTP Context Accessor ────────────────────────────────────────────────────
builder.Services.AddHttpContextAccessor();

// ── Custom Services ──────────────────────────────────────────────────────────
// Scoped because they depend on DbContext (which is Scoped)
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
// Singleton — no HttpContext or DbContext dependency
builder.Services.AddSingleton<EmailService>();
// Scoped — uses Session/Cookie (HttpContext-bound)
builder.Services.AddScoped<CartService>();

var app = builder.Build();

// ── Auto-apply Migrations on startup ────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();        // creates DB + tables if not exists
    DbInitializer.Seed(db);       // inserts initial products only if table is empty
}

// ── HTTP Pipeline ────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
