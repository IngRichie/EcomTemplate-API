namespace GrocerySupermarket.Domain.Entities;

public class ContactMessage
{
    public Guid ContactMessageId { get; set; }

    // =========================
    // CONTACT DETAILS
    // =========================
    public required string FullName { get; set; }

    public required string Email { get; set; }

    public string? PhoneNumber { get; set; }

    // =========================
    // MESSAGE
    // =========================
    public required string Subject { get; set; }

    public required string Message { get; set; }

    // =========================
    // STATUS
    // =========================
    public bool IsRead { get; set; } = false;

    // =========================
    // AUDIT
    // =========================
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}