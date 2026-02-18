namespace GrocerySupermarket.Application.DTOs;

public class CustomerReviewDTO
{
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
}
