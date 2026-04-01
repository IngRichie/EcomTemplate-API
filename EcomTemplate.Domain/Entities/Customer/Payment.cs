using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

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

    [Column(TypeName = "numeric(10,2)")]
    public decimal Amount { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    public Order Order { get; set; } = null!;

    public DateTime CreatedAt {get; set;}
}