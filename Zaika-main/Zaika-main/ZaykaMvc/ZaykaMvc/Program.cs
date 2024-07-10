using Microsoft.EntityFrameworkCore;
using ZaykaMvc.Models;

namespace ZaykaMvc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<ZaykaDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

        builder.Services.AddSession(option => {
            option.IdleTimeout = TimeSpan.FromHours(10);
            option.Cookie.HttpOnly = true;
            option.Cookie.IsEssential = true;
        });

        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();
        app.UseSession();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=User}/{action=SignUp}/{id?}");

        app.Run();
    }
}

