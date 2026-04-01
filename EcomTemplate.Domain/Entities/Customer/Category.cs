using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("categories")]
public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CategoryId { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}