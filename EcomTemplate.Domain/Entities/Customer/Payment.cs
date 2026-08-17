using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomTemplate.Domain.Entities;

[Table("payments")]
public class Payment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PaymentId { get; set; }

    [Required]
    public Guid OrderId { get; set; }

    [Required, MaxLength(50)]
    public string Provider { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string ProviderReference { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? ProviderTransactionId { get; set; }

    [Column(TypeName = "numeric(10,2)")]
    public decimal Amount { get; set; }

    [Required, MaxLength(10)]
    public string Currency { get; set; } = "GHS";

    [Required, MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Channel { get; set; }

    [MaxLength(500)]
    public string? FailureReason { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool InventoryFinalized { get; set; }

    public Order Order { get; set; } = null!;

    public DateTime CreatedAt {get; set;}
}
