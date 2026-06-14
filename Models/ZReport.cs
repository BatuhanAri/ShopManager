using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopManager.Models;

public class ZReport
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Tarih")]
    public DateTime ReportDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Fiyat (₺)")]
    public decimal Price { get; set; }
}
