public class ProductVariantAttributeDTO
{
    public Guid ProductVariantAttributeId { get; set; }

    public string Name { get; set; } = null!;   // e.g. "Color"
    public string Value { get; set; } = null!;  // e.g. "Blue"
}
