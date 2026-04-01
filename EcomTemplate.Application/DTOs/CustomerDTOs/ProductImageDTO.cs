namespace GrocerySupermarket.Application.DTOs;

public class ProductImageDTO
{
    public Guid ImageId { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}
