using AliAndNinoClone.DAL;
using AliAndNinoClone.DAL;
using AliAndNinoClone.Services;
using Microsoft.EntityFrameworkCore;

namespace AliAndNinoClone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
     options.UseSqlServer("Server=DESKTOP-MG2DRQ7;Database=AliAndNinoCloneDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;"));

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<BookApiService>();
            builder.Services.AddHttpClient(); // BU SƏTİR ÇOX VACİBDİR
            builder.Services.AddScoped<GeminiApiService>();
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
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