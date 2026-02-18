using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("customer_profiles")]
public class CustomerProfile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CustomerProfileId { get; set; }

    // =======================
    // PERSONAL INFO
    // =======================

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [MaxLength(255)]
    public string? Email { get; set; }

    [MaxLength(30)]
    public string? Phone { get; set; }

    // 🖼️ Profile image
    [MaxLength(500)]
    public string? ProfileImageUrl { get; set; }
    public string? Gender {get; set;}

    // =======================
    // DELIVERY INFO
    // =======================

    [MaxLength(150)]
    public string? UniversityName { get; set; }

    [MaxLength(150)]
    public string? HostelName { get; set; }

    [MaxLength(50)]
    public string? RoomNumber { get; set; }

/// <summary>
/// Optional delivery instructions provided by the customer to help the rider
/// locate the delivery point easily within a campus or residence.
/// 
/// Examples:
/// - "Hostel B, Block 2, Room 204"
/// - "Call me when you reach the main gate"
/// - "Deliver to the porter, I’ll come down"
/// - "Second gate beside the science faculty"
/// - "Do not knock, call instead"
/// </summary>
[MaxLength(500)]
public string? DeliveryInstructions { get; set; }


    

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // =======================
    // NAVIGATION
    // =======================

    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public CustomerAuth? Auth { get; set; }

    public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();

}
