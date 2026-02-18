

namespace GrocerySupermarket.Application.DTOs;


public class CheckoutRequestDTO
{
    public Guid? CustomerProfileId { get; set; }

    public Guid? GuestUserId { get; set; }

    // 🔐 Required for guest checkout
    public string? DeviceId { get; set; }

    // DELIVERY INFO
    public string? UniversityName { get; set; }
    public string? HostelName { get; set; }
    public string? RoomNumber { get; set; }
    public string? DeliveryInstructions { get; set; }

    public string? ReferralCode { get; set; }
    public bool UseDelivery { get; set; } = true;
}

