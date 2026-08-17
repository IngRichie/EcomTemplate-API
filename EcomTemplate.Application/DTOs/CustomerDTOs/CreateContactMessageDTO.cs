namespace GrocerySupermarket.Application.DTOs.ContactMessage;

public class CreateContactMessageDTO
{
    public required string FullName { get; set; }

    public required string Email { get; set; }

    public string? PhoneNumber { get; set; }

    public required string Subject { get; set; }

    public required string Message { get; set; }
}