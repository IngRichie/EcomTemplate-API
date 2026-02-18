using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("carts")]
public class Cart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CartId { get; set; }

    // =======================
    // OWNERSHIP (ONE OF THESE)
    // =======================
    public Guid? CustomerProfileId { get; set; }
    public CustomerProfile? CustomerProfile { get; set; }

    public Guid? GuestUserId { get; set; }
    public GuestUser? GuestUser { get; set; }

    // Device-based tracking (important for guests)
    [MaxLength(255)]
    public string? DeviceId { get; set; }

    // =======================
    // CART STATE
    // =======================
    public bool IsCheckedOut { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // =======================
    // ITEMS
    // =======================
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
