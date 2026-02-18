using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("home_ad")]
public class HomeAd
{
    [Key] public Guid Id { get; set; }
    public string SectionKey { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? CtaText { get; set; }
    public string? CtaUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}