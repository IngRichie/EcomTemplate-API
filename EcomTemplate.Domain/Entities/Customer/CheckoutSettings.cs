using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GrocerySupermarket.Domain.Entities;

[Table("checkout_settings")]
public class CheckoutSettings
{
    [Key]
    public Guid Id { get; set; }

    [Column(TypeName = "numeric(5,2)")]
    public decimal TaxPercentage { get; set; }

    [Column(TypeName = "numeric(10,2)")]
    public decimal DeliveryFee { get; set; }

    [Column(TypeName = "numeric(5,2)")]
    public decimal ReferralDiscountPercentage { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
