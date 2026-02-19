namespace GrocerySupermarket.Application.DTOs;

public class ProductDTO
{
    // Core
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

   
  

    // Media & Reviews
    public List<ProductImageDTO> Images { get; set; } = new();
    public List<ProductReviewDTO> Reviews { get; set; } = new();
    public List<ProductVariantDTO> ProductVariants { get; set; } = new();
}
