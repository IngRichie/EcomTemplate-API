using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities.Admin;

[Table("admin_profile")]
public class AdminProfile
{
    [Key]
    public Guid AdminId { get; set; } = Guid.NewGuid();

    // =========================
    // BASIC INFO
    // =========================
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    // =========================
    // SECURITY
    // =========================
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    // Roles: SuperAdmin, Manager, Support
    [Required]
    [MaxLength(50)]
    public string Role { get; set; } = "Admin";

    public bool IsActive { get; set; } = true;

    // =========================
    // LOGIN TRACKING
    // =========================
    public DateTime? LastLoginAt { get; set; }

    public int FailedLoginAttempts { get; set; } = 0;

    public DateTime? LockoutEnd { get; set; }

    // =========================
    // AUDIT
    // =========================
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}