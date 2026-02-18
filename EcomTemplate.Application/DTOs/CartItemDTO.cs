namespace GrocerySupermarket.Application.DTOs;

public class CartItemDTO
{
    public Guid CartItemId { get; set; }
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public List<string> Images { get; set; } = new();
    public decimal TotalPrice => UnitPrice * Quantity;
}
