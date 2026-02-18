using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("referrals")]
public class Referral
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ReferralId { get; set; }

    [Required]
    public Guid ReferrerCustomerId { get; set; }

    [Required]
    public string ReferralCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRedeemed { get; set; }
}