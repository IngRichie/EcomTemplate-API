namespace GrocerySupermarket.Application.DTOs;
public class CheckoutSummaryDTO
{
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal Total { get; set; }

    public string Currency { get; set; } = "GH₵";
}
