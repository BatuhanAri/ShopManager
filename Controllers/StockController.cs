using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManager.Models;

namespace ShopManager.Controllers;

public class StockController : Controller
{
    private readonly AppDbContext _db;

    public StockController(AppDbContext db)
    {
        _db = db;
    }

    // GET: /Stock
    public async Task<IActionResult> Index()
    {
        var stocks = await _db.Stocks.OrderBy(s => s.ItemName).ToListAsync();
        return View(stocks);
    }

    // GET: /Stock/Create
    public IActionResult Create() => View();

    // POST: /Stock/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Stock stock)
    {
        if (!ModelState.IsValid) return View(stock);

        _db.Stocks.Add(stock);
        await _db.SaveChangesAsync();
        TempData["Success"] = $"'{stock.ItemName}' stok kaydı oluşturuldu.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Stock/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var stock = await _db.Stocks.FindAsync(id);
        if (stock == null) return NotFound();
        return View(stock);
    }

    // POST: /Stock/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Stock stock)
    {
        if (id != stock.Id) return BadRequest();
        if (!ModelState.IsValid) return View(stock);

        _db.Stocks.Update(stock);
        await _db.SaveChangesAsync();
        TempData["Success"] = $"'{stock.ItemName}' güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /Stock/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var stock = await _db.Stocks.FindAsync(id);
        if (stock == null) return NotFound();

        _db.Stocks.Remove(stock);
        await _db.SaveChangesAsync();
        TempData["Success"] = $"'{stock.ItemName}' silindi.";
        return RedirectToAction(nameof(Index));
    }

    // ── AJAX Endpoint ─────────────────────────────────────────────────────────
    // POST: /Stock/AdjustQuantity
    // Body: { "id": 1, "delta": 5.0 }
    [HttpPost]
    public async Task<IActionResult> AdjustQuantity([FromBody] AdjustRequest req)
    {
        if (req == null) return BadRequest(new { message = "Geçersiz istek." });

        var stock = await _db.Stocks.FindAsync(req.Id);
        if (stock == null) return NotFound(new { message = "Stok kalemi bulunamadı." });

        var newQty = stock.CurrentQuantity + req.Delta;
        if (newQty < 0)
            return BadRequest(new { message = "Miktar sıfırın altına düşemez." });

        stock.CurrentQuantity = newQty;
        await _db.SaveChangesAsync();

        return Ok(new
        {
            id             = stock.Id,
            itemName       = stock.ItemName,
            currentQuantity = stock.CurrentQuantity,
            unit           = stock.Unit,
            isCritical     = stock.IsCritical,
            delta          = req.Delta
        });
    }

    public record AdjustRequest(int Id, decimal Delta);
}
