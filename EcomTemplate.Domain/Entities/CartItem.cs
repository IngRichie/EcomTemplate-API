using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("cart_items")]
public class CartItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CartItemId { get; set; }

    [Required]
    public Guid CartId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid ProductVariantId { get; set; }

    public int Quantity { get; set; }

    // Navigation
    public Product Product { get; set; } = null!;
    public ProductVariant ProductVariant { get; set; } = null!;

    // ===== Derived (NOT persisted) =====
    [NotMapped]
    public string ProductName => Product.Name;

    [NotMapped]
    public decimal UnitPrice => ProductVariant.Price;

    [NotMapped]
    public decimal TotalPrice => UnitPrice * Quantity;
}
