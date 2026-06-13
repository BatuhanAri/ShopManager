using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopManager.Models;

public class DailyTurnover
{
    public int Id { get; set; }

    [Display(Name = "Tarih")]
    public DateTime RecordDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Nakit (₺)")]
    public decimal CashAmount { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Kart (₺)")]
    public decimal CardAmount { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "E-Ticaret (₺)")]
    public decimal ECommerceAmount { get; set; }

    [StringLength(500)]
    [Display(Name = "Notlar")]
    public string? Notes { get; set; }

    // Computed helper – not mapped
    [NotMapped]
    public decimal TotalAmount => CashAmount + CardAmount + ECommerceAmount;
}
