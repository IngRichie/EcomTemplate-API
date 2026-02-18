using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("video_ads")]
public class VideoAd
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public string VideoUrl { get; set; } = string.Empty;

    public string? ThumbnailUrl { get; set; }

    public string? TargetUrl { get; set; }

    public bool IsSkippable { get; set; } = true;

    public int SkipAfterSeconds { get; set; } = 5;

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
