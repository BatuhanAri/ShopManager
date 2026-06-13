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

    // GET: /DailyTurnover
    public async Task<IActionResult> Index()
    {
        var records = await _db.DailyTurnovers
            .OrderByDescending(t => t.RecordDate)
            .ToListAsync();
        return View(records);
    }

    // GET: /DailyTurnover/Create
    public IActionResult Create()
    {
        var model = new DailyTurnover { RecordDate = DateTime.UtcNow };
        return View(model);
    }

    // POST: /DailyTurnover/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DailyTurnover turnover)
    {
        if (!ModelState.IsValid) return View(turnover);

        turnover.RecordDate = DateTime.SpecifyKind(turnover.RecordDate, DateTimeKind.Utc);
        _db.DailyTurnovers.Add(turnover);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Günlük ciro kaydedildi.";
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
}
