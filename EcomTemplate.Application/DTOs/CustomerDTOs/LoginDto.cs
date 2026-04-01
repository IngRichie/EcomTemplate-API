using System.ComponentModel.DataAnnotations;

namespace GrocerySupermarket.Application.DTOs;

public class LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// If true, issue long-lived refresh token
    /// </summary>
    public bool RememberMe { get; set; } = false;
}
