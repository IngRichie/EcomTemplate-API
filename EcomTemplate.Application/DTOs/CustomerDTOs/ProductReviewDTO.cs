namespace GrocerySupermarket.Application.DTOs;

public class ProductReviewDTO
{
    public Guid ProductReviewId { get; set; }

    public Guid ProductId { get; set; }
    public Guid CustomerProfileId { get; set; }

    public int Rating { get; set; }
    public string? Comment { get; set; }

    // Optional navigation data (safe to include)
    public CustomerReviewDTO? ReviewerInfo { get; set; }
}
