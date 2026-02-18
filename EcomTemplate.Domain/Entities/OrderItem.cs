using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("order_items")]
public class OrderItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid OrderItemId { get; set; }

    [Required]
    public Guid OrderId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "numeric(10,2)")]
    public decimal UnitPrice { get; set; }

    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}