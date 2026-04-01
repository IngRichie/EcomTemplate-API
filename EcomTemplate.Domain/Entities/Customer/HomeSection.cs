using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("home_section")]
public class HomeSection
{
    [Key] public Guid Id { get; set; }
    public string Key { get; set; } = null!;
    public string? Title { get; set; }
    public string Type { get; set; } = null!;
    public int Position { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}