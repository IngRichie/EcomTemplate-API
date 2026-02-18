using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("guest_users")]
public class GuestUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid GuestUserId { get; set; }

    // =======================
    // DEVICE TRACKING
    // =======================

    [Required, MaxLength(255)]
    public string DeviceId { get; set; } = null!;

    // =======================
    // PERSONAL INFO (OPTIONAL)
    // =======================

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [MaxLength(255)]
    public string? Email { get; set; }

    [MaxLength(30)]
    public string? Phone { get; set; }

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



    // =======================
    // SYSTEM
    // =======================

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
