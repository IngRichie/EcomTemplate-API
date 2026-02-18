using System.ComponentModel.DataAnnotations;

namespace GrocerySupermarket.Application.DTOs;

public class UpdateCustomerProfileDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? Email { get; set; }
    public string? Phone { get; set; }

    // 🖼️ Profile image
    [MaxLength(500)]
    public string? ProfileImageUrl { get; set; }

    public string? Gender {get; set;}

    public string? UniversityName { get; set; }
    public string? HostelName { get; set; }
    public string? RoomNumber { get; set; }
    public string? DeliveryInstructions { get; set; }
}
