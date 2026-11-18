<<<<<<< HEAD
=======

>>>>>>> 19e1c65 (18.11.2026)
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
<<<<<<< HEAD
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});
// DbContext Qeydiyyatı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/admin/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.Cookie.HttpOnly = true; // XSS hücumlarından qoruyur
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // HTTPS-i dəstəkləyir
        options.Cookie.SameSite = SameSiteMode.Strict; // CSRF hücumlarının qarşısını alır
    });
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<WebApplication1.Services.IGeoIpService, WebApplication1.Services.GeoIpService>();
builder.Services.AddControllersWithViews();
=======

// Forwarded Headers
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    // KnownProxies və KnownIPNetworks silinmir.
    // Yalnız etibarlı proxy-lər qəbul edilməlidir.
});

// Database
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
// Authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/admin/login";
        options.AccessDeniedPath = "/admin/login";

        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;

        options.Cookie.Name = "GameBack.AdminAuth";
        options.Cookie.HttpOnly = true;

        options.Cookie.SecurePolicy =
            builder.Environment.IsDevelopment()
                ? CookieSecurePolicy.SameAsRequest
                : CookieSecurePolicy.Always;

        options.Cookie.SameSite = SameSiteMode.Lax;
    });

builder.Services.AddAuthorization();

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<
    WebApplication1.Services.IGeoIpService,
    WebApplication1.Services.GeoIpService
>();

builder.Services.AddControllersWithViews();

>>>>>>> 19e1c65 (18.11.2026)
// Services
builder.Services.AddScoped<ICasinoService, CasinoService>();

var app = builder.Build();

<<<<<<< HEAD
// Seed data vasitəsilə ilk başlanğıc məlumatlarını əlavə etmək
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
=======
// Middleware sırası
app.UseForwardedHeaders();

>>>>>>> 19e1c65 (18.11.2026)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}
<<<<<<< HEAD
app.UseForwardedHeaders();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
=======

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

// Routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
>>>>>>> 19e1c65 (18.11.2026)
