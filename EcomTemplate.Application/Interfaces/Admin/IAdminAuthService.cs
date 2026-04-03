using EcomTemplate.Application.DTOs.AdminDTOs;

namespace EcomTemplate.Application.Interfaces.Admin;



public interface IAdminAuthService
{
    // =======================
    // REGISTER ADMIN
    // =======================
    Task<AdminAuthResponseDto> Register(AdminRegisterDto dto);

    // =======================
    // LOGIN ADMIN
    // =======================
    Task<AdminAuthResponseDto> Login(AdminLoginDto dto);

    // =======================
    // REFRESH TOKEN
    // =======================
    Task<AdminAuthResponseDto> RefreshToken(string refreshToken);
}