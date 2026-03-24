namespace GrocerySupermarket.Application.DTOs;

public class CustomerDTO
{
    // public Guid CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? ProfileImageUrl { get; set; }

    public string? Location { get; set; }

    public DateTime CreatedAt { get; set; }
}
