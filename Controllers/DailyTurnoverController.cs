using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManager.Models;

namespace ShopManager.Controllers;

public class DailyTurnoverController : Controller
{
    private readonly AppDbContext _db;

    public DailyTurnoverController(AppDbContext db)
    {
        _db = db;
    }

    // GET: /DailyTurnover?startDate=2024-01-01&endDate=2024-12-31
    public async Task<IActionResult> Index(
        DateTime? startDate,
        DateTime? endDate,
        int? year,
        int? month)
    {
        // Build available years for filter dropdown
        var allYears = await _db.DailyTurnovers
            .Select(t => t.RecordDate.Year)
            .Distinct()
            .OrderByDescending(y => y)
            .ToListAsync();

        // Add current year even if no records yet
        if (!allYears.Contains(DateTime.UtcNow.Year))
            allYears.Insert(0, DateTime.UtcNow.Year);

        ViewBag.AvailableYears = allYears;
        ViewBag.SelectedYear   = year;
        ViewBag.SelectedMonth  = month;
        ViewBag.StartDate      = startDate?.ToString("yyyy-MM-dd");
        ViewBag.EndDate        = endDate?.ToString("yyyy-MM-dd");

        var query = _db.DailyTurnovers.AsQueryable();

        // Apply filters
        if (year.HasValue && month.HasValue)
        {
            query = query.Where(t => t.RecordDate.Year  == year.Value
                                  && t.RecordDate.Month == month.Value);
        }
        else if (year.HasValue)
        {
            query = query.Where(t => t.RecordDate.Year == year.Value);
        }
        else if (startDate.HasValue || endDate.HasValue)
        {
            if (startDate.HasValue)
            {
                var utcStart = DateTime.SpecifyKind(startDate.Value.Date, DateTimeKind.Utc);
                query = query.Where(t => t.RecordDate >= utcStart);
            }
            if (endDate.HasValue)
            {
                var utcEnd = DateTime.SpecifyKind(endDate.Value.Date.AddDays(1), DateTimeKind.Utc);
                query = query.Where(t => t.RecordDate < utcEnd);
            }
        }

        var records = await query
            .OrderByDescending(t => t.RecordDate)
            .ToListAsync();

        ViewBag.ZReports = await _db.ZReports
            .OrderByDescending(z => z.ReportDate)
            .ToListAsync();

        return View(records);
    }

    // GET: /DailyTurnover/Create?date=2024-06-01
    public IActionResult Create(DateTime? date)
    {
        var model = new DailyTurnover
        {
            RecordDate = date.HasValue
                ? DateTime.SpecifyKind(date.Value.Date, DateTimeKind.Utc)
                : DateTime.UtcNow
        };
        return View(model);
    }

    // POST: /DailyTurnover/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DailyTurnover turnover)
    {
        if (!ModelState.IsValid) return View(turnover);

        turnover.RecordDate = DateTime.SpecifyKind(turnover.RecordDate.Date, DateTimeKind.Utc);
        _db.DailyTurnovers.Add(turnover);
        await _db.SaveChangesAsync();

        var isHistorical = turnover.RecordDate.Date < DateTime.UtcNow.Date;
        TempData["Success"] = isHistorical
            ? $"{turnover.RecordDate:dd.MM.yyyy} tarihli geçmiş ciro kaydı kaydedildi."
            : "Günlük ciro kaydedildi.";

        return RedirectToAction(nameof(Index));
    }

    // POST: /DailyTurnover/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var record = await _db.DailyTurnovers.FindAsync(id);
        if (record == null) return NotFound();

        _db.DailyTurnovers.Remove(record);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Kayıt silindi.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /DailyTurnover/CreateZReport
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateZReport(DateTime date, decimal price)
    {
        if (price <= 0)
        {
            TempData["Success"] = "Hata: Geçersiz fiyat.";
            return RedirectToAction(nameof(Index));
        }

        var zReport = new ZReport
        {
            ReportDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc),
            Price = price
        };

        _db.ZReports.Add(zReport);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"{zReport.ReportDate:dd.MM.yyyy} tarihli Z raporu kaydedildi.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /DailyTurnover/DeleteZReport/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteZReport(int id)
    {
        var record = await _db.ZReports.FindAsync(id);
        if (record == null) return NotFound();

        _db.ZReports.Remove(record);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Z raporu kaydı silindi.";
        return RedirectToAction(nameof(Index));
    }
}

