using GrocerySupermarket.Application.DTOs;

public class ProductSummaryDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ProductImageDTO? PrimaryImage { get; set; }
}
