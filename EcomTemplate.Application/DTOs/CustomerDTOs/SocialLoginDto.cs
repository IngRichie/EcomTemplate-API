using System.ComponentModel.DataAnnotations;

namespace GrocerySupermarket.Application.DTOs;

public class SocialLoginDto
{
    [Required]
    public SocialProvider Provider { get; set; }

    /// <summary>
    /// ID token returned by Google or Apple
    /// </summary>
    [Required]
    public string IdToken { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = true;
}

public enum SocialProvider
{
    Google,
    Apple
}
