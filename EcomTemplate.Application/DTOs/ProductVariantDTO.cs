namespace GrocerySupermarket.Application.DTOs;

public class ProductVariantDTO
{
    public Guid ProductVariantId { get; set; }

    public string Sku { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public List<ProductVariantAttributeDTO> Attributes { get; set; }
        = new List<ProductVariantAttributeDTO>();
}
