namespace GrocerySupermarket.Application.DTOs;

public class CustomerDTO
{
    public Guid CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? ProfileImageUrl { get; set; }

    // =======================
    // DELIVERY LOCATION
    // =======================

    /// <summary>
    /// Name of the university or campus (e.g. "University of Lagos")
    /// </summary>
    public string? University { get; set; }

    /// <summary>
    /// Hostel or residence name (e.g. "Moremi Hall", "Hostel B")
    /// </summary>
    public string? HostelName { get; set; }

    /// <summary>
    /// Optional human-readable delivery instructions
    /// (e.g. "Room 204, call when you arrive")
    /// </summary>
    public string? DeliveryInstructions { get; set; }

    public DateTime CreatedAt { get; set; }
}
