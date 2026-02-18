using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("referral_codes")]
public class ReferralCode
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ReferrerUserId { get; set; }

    [Required, MaxLength(50)]
    public string Code { get; set; } = null!;

    [Required]
    public DiscountType DiscountType { get; set; }

    [Column(TypeName = "numeric(18,2)")]
    public decimal DiscountValue { get; set; }

    public int MaxUses { get; set; } = 1;

    public DateTime? ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }
}

public enum DiscountType
{
    Percentage,
    Fixed
}
