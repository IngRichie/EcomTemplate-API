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
    public string? Location { get; set; }



    

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
