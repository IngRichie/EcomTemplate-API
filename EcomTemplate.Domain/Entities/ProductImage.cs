using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("product_images")]
public class ProductImage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProductImageId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required, MaxLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    public bool IsPrimary { get; set; } 

    public Product Product { get; set; } = null!;
}