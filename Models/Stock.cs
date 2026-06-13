using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopManager.Models;

public class Stock
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ürün adı zorunludur.")]
    [StringLength(150)]
    [Display(Name = "Ürün Adı")]
    public string ItemName { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,3)")]
    [Display(Name = "Mevcut Miktar")]
    public decimal CurrentQuantity { get; set; }

    [Required(ErrorMessage = "Birim zorunludur.")]
    [StringLength(20)]
    [Display(Name = "Birim")]
    public string Unit { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,3)")]
    [Display(Name = "Kritik Limit")]
    public decimal CriticalLimit { get; set; }

    // Computed helper – not mapped
    [NotMapped]
    public bool IsCritical => CurrentQuantity <= CriticalLimit;
}
