namespace GrocerySupermarket.Application.DTOs.AdminDTOs;
public class AdminAuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public int ExpiresIn { get; set; }

    public AdminProfileDTO Admin { get; set; } = new();
}