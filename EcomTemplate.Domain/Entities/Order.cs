using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;


[Table("orders")]
public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid OrderId { get; set; }

    // REGISTERED USER (nullable)
    public Guid? CustomerProfileId { get; set; }

    // GUEST USER (nullable)
    public Guid? GuestUserId { get; set; }

    // =======================
    // SNAPSHOT VALUES
    // =======================

    [Column(TypeName = "numeric(10,2)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "numeric(10,2)")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "numeric(10,2)")]
    public decimal TaxAmount { get; set; }

    [Column(TypeName = "numeric(10,2)")]
    public decimal DeliveryFee { get; set; }

    [Column(TypeName = "numeric(10,2)")]
    public decimal TotalAmount { get; set; }

    [Required, MaxLength(30)]
    public string Status { get; set; } = "pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // =======================
    // NAVIGATION
    // =======================

    public CustomerProfile? CustomerProfile { get; set; }
    public GuestUser? GuestUser { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public Payment? Payment { get; set; }
}
