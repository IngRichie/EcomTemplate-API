namespace GrocerySupermarket.Application.DTOs; 
public class InitializePaymentDTO
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public required string Email { get; set; }
}