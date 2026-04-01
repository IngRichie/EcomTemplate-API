namespace GrocerySupermarket.Application.DTOs;

public class CheckoutResult
{
    public Guid CustomerId { get; set; }

    // Cart summary
    public decimal Subtotal { get; set; }

    // Discounts
    public string? AppliedReferralCode { get; set; }
    public decimal DiscountAmount { get; set; }

    // Charges
    public decimal TaxAmount { get; set; }
    public decimal DeliveryFee { get; set; }

    // Final
    public decimal TotalAmount { get; set; }

    // Status
    public bool IsValid { get; set; }
    public string? Message { get; set; }
}
