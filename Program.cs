using AliAndNinoClone.DAL;
using AliAndNinoClone.Models.Common; // AppUser üçün
using AliAndNinoClone.Services;
using Microsoft.AspNetCore.Identity; // Identity üçün
using Microsoft.EntityFrameworkCore;

namespace AliAndNinoClone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. MVC Kontrollerlərini əlavə edirik
            builder.Services.AddControllersWithViews();

            // 2. Verilənlər Bazası (DbContext) Tənzimləməsi
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
                "Server=DESKTOP-MG2DRQ7;Database=AliAndNinoCloneDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;"));

            // 3. Identity (İstifadəçi Sistemi) Tənzimləmələri
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // Şifrə təhlükəsizlik qaydaları
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;

                // E-poçt təkrar oluna bilməz
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // 4. Cookie (Giriş Session-u) Tənzimləmələri
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login"; // Giriş edilməyibsə bura göndər
                options.AccessDeniedPath = "/Account/AccessDenied"; // İcazə yoxdursa bura göndər
                options.Cookie.Name = "AliAndNinoAuth";
                options.ExpireTimeSpan = TimeSpan.FromDays(5); // 5 gün yadda saxla
            });

            // 5. Servislərin Qeydiyyatı
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<BookApiService>();
            builder.Services.AddScoped<GeminiApiService>();

            var app = builder.Build();

            // Middleware-lər
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // 6. VACİB: Əvvəl Authentication, sonra Authorization gəlməlidir!
            app.UseAuthentication(); // "Sən kimsən?" sualına cavab verir
            app.UseAuthorization();  // "Bura girməyə icazən var?" sualına cavab verir

            // Marşrutlar (Routing)
            app.MapControllerRoute(
                 name: "areas",
                 pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}