namespace GrocerySupermarket.Application.DTOs;

public class GuestUserDTO
{
    public Guid Id { get; set; }

    /// <summary>
    /// Unique device identifier for guest tracking
    /// </summary>
    public string DeviceId { get; set; } = string.Empty;

    // =======================
    // DELIVERY LOCATION
    // =======================

    public string? University { get; set; }

    public string? HostelName { get; set; }

    public string? DeliveryInstructions { get; set; }

    public DateTime CreatedAt { get; set; }
}
