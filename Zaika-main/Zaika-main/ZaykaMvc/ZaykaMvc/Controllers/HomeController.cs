using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZaykaMvc.Models;

namespace ZaykaMvc.Controllers;

public class HomeController : Controller
{
    private readonly ZaykaDbContext context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ZaykaDbContext context)
    {
        _logger = logger;
        this.context = context;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("username") == null) {
            return RedirectToAction("Login", "User");
        }
        return context.Dishes != null ?
                        View(await context.Dishes.ToListAsync()):
                        Problem("Entity set 'ZaykaDbContext.Users'  is null.");
    }

    public IActionResult AboutUs()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

