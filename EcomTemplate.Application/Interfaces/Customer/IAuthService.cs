using GrocerySupermarket.Application.DTOs;

public interface IAuthService
{
    // Email/password
    Task<AuthResponseDto> Register(CustomerDTO dto, string password);
    Task<AuthResponseDto> Login(LoginDto dto);

    // Social login
    Task<AuthResponseDto> SocialLogin(SocialLoginDto dto);

    // Tokens
    Task<AuthResponseDto> RefreshToken(string refreshToken);
}
