using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("banners")]
public class Banner
{
    [Key]
    public Guid Id { get; set; }

    [Required] public string Title { get; set; } = null!;
    public string? Description { get; set; }
    [Required] public string ImageUrl { get; set; } = null!;
    public string? TargetUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}