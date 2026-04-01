using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("home_section_product")]
public class HomeSectionProduct
{
    [Key] public Guid Id { get; set; }
    public string SectionKey { get; set; } = null!;
    public Guid ProductId { get; set; }
    public int Position { get; set; }
}