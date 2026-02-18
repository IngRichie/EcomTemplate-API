using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("category_promo")]
public class CategoryPromo
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Title { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }
}