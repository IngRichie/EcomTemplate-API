namespace GrocerySupermarket.Application.DTOs;

public class CartDTO
{
    public Guid CartId { get; set; }
    public Guid CustomerProfileId { get; set; }

    public List<CartItemDTO> Items { get; set; } = new();
}
