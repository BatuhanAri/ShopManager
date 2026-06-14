using Microsoft.EntityFrameworkCore;

namespace ShopManager.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Stock> Stocks { get; set; }
    public DbSet<DailyTurnover> DailyTurnovers { get; set; }
    public DbSet<ZReport> ZReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed some initial stock items
        modelBuilder.Entity<Stock>().HasData(
            new Stock { Id = 1, ItemName = "Çilek", CurrentQuantity = 10, Unit = "Kg", CriticalLimit = 3 },
            new Stock { Id = 2, ItemName = "Muz", CurrentQuantity = 8, Unit = "Kg", CriticalLimit = 2 },
            new Stock { Id = 3, ItemName = "Waffle Hamuru", CurrentQuantity = 5, Unit = "Kg", CriticalLimit = 2 },
            new Stock { Id = 4, ItemName = "Dondurma (Çikolata)", CurrentQuantity = 6, Unit = "Litre", CriticalLimit = 2 },
            new Stock { Id = 5, ItemName = "Dondurma (Vanilya)", CurrentQuantity = 4, Unit = "Litre", CriticalLimit = 2 },
            new Stock { Id = 6, ItemName = "Çikolata Sos", CurrentQuantity = 2, Unit = "Litre", CriticalLimit = 1 },
            new Stock { Id = 7, ItemName = "Kağıt Kap (M)", CurrentQuantity = 200, Unit = "Adet", CriticalLimit = 50 },
            new Stock { Id = 8, ItemName = "Kağıt Kap (L)", CurrentQuantity = 150, Unit = "Adet", CriticalLimit = 50 }
        );
    }
}
