using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("vendor_mobile_hero_section")]
public class VendorMobileHero
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public Guid VendorId { get; set; }

    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    public string? TargetUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    /* Navigation */
    public Vendor Vendor { get; set; } = null!;
}
