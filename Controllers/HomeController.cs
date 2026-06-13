using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManager.Models;

namespace ShopManager.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        // Today's turnover (UTC date match)
        var today = DateTime.UtcNow.Date;
        var todayRecords = await _db.DailyTurnovers
            .Where(t => t.RecordDate.Date == today)
            .ToListAsync();

        ViewBag.TodayCash        = todayRecords.Sum(t => t.CashAmount);
        ViewBag.TodayCard        = todayRecords.Sum(t => t.CardAmount);
        ViewBag.TodayECommerce   = todayRecords.Sum(t => t.ECommerceAmount);
        ViewBag.TodayTotal       = todayRecords.Sum(t => t.CashAmount + t.CardAmount + t.ECommerceAmount);

        // Critical stock items
        var criticalStocks = await _db.Stocks
            .Where(s => s.CurrentQuantity <= s.CriticalLimit)
            .OrderBy(s => s.ItemName)
            .ToListAsync();

        ViewBag.CriticalStocks = criticalStocks;

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}
