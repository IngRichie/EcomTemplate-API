namespace GrocerySupermarket.Application.DTOs;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;

    public int ExpiresIn { get; set; } // seconds

    public CustomerDTO Customer { get; set; } = null!;
}
