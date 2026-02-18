using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("vendor_hero_sections")]
public class VendorHeroSection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public Guid VendorId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Subtitle { get; set; }

    [Required]
    public string MediaUrl { get; set; } = string.Empty; // image or video

    [Required]
    public string MediaType { get; set; } = "image"; // image | video

    public string? CtaText { get; set; }

    public string? CtaUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    /* Navigation */
    public Vendor Vendor { get; set; } = null!;
}
