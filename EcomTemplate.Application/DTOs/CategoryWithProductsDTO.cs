using GrocerySupermarket.Application.DTOs;

public class ProductSummaryDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
    public ProductImageDTO? PrimaryImage { get; set; }

    public ProductVariantDTO? Variant { get; set; }
}
