using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("products")]
public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProductId { get; set; }

    [Required]
    public Guid VendorId { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

//    [Column(TypeName = "numeric(10,2)")]
   // public decimal Price { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public Vendor Vendor { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();

    public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
}
