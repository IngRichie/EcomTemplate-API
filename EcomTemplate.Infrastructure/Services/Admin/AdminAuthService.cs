using EcomTemplate.Infrastructure.Services.Admin;
using EcomTemplate.Application.DTOs.AdminDTOs;
using EcomTemplate.Application.Interfaces.Admin;
using EcomTemplate.Domain.Entities;
using EcomTemplate.Domain.Entities.Admin;
using EcomTemplate.Infrastructure.Data;
using EcomTemplate.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcomTemplate.Infrastructure.Services.Admin;

public class AdminAuthService : IAdminAuthService
{
    private readonly AppDbContext _db;
    private readonly AdminTokenService _tokenService;
    private readonly PasswordHasher<AdminProfile> _passwordHasher;

    public AdminAuthService(AppDbContext db, AdminTokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
        _passwordHasher = new PasswordHasher<AdminProfile>();
    }

    // =======================
    // REGISTER ADMIN
    // =======================
    public async Task<AdminAuthResponseDto> Register(AdminRegisterDto dto)
    {
        var existing = await _db.AdminProfile
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == dto.Email);

        if (existing != null)
            throw new Exception("Email already registered.");

        var admin = new AdminProfile
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Role = dto.Role ?? "Admin",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        admin.PasswordHash = _passwordHasher.HashPassword(admin, dto.Password);

        _db.AdminProfile.Add(admin);
        await _db.SaveChangesAsync();

        return await GenerateAuthResponse(admin, false);
    }

    // =======================
    // LOGIN ADMIN
    // =======================
    public async Task<AdminAuthResponseDto> Login(AdminLoginDto dto)
    {
        var admin = await _db.AdminProfile
            .FirstOrDefaultAsync(x => x.Email == dto.Email)
            ?? throw new UnauthorizedException("Invalid email or password.");

        if (admin.LockoutEnd != null && admin.LockoutEnd > DateTime.UtcNow)
            throw new UnauthorizedException("Account is locked. Try again later.");

        var result = _passwordHasher.VerifyHashedPassword(
            admin,
            admin.PasswordHash,
            dto.Password
        );

        if (result == PasswordVerificationResult.Failed)
        {
            admin.FailedLoginAttempts++;

            if (admin.FailedLoginAttempts >= 5)
                admin.LockoutEnd = DateTime.UtcNow.AddMinutes(15);

            await _db.SaveChangesAsync();
            throw new UnauthorizedException("Invalid email or password.");
        }

        admin.FailedLoginAttempts = 0;
        admin.LockoutEnd = null;
        admin.LastLoginAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return await GenerateAuthResponse(admin, dto.RememberMe);
    }

    // =======================
    // REFRESH TOKEN
    // =======================
    public async Task<AdminAuthResponseDto> RefreshToken(string refreshToken)
    {
        var token = await _db.RefreshTokens
            .Include(x => x.AdminProfile)
            .FirstOrDefaultAsync(x => x.Token == refreshToken);

        if (token == null || token.IsRevoked || token.ExpiresAt < DateTime.UtcNow)
            throw new Exception("Invalid refresh token.");

        token.IsRevoked = true;
        await _db.SaveChangesAsync();

        return await GenerateAuthResponse(token.AdminProfile!, false);
    }

    // =======================
    // GENERATE RESPONSE
    // =======================
    private async Task<AdminAuthResponseDto> GenerateAuthResponse(
        AdminProfile admin,
        bool rememberMe)
    {
        var accessToken = _tokenService.CreateAdminAccessToken(
            admin.AdminId,
            admin.Email,
            admin.Role
        );

        var refreshToken = _tokenService.CreateRefreshToken(
            admin.AdminId,
            rememberMe
        );

        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync();

        return new AdminAuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = 3600,
            Admin = new AdminProfileDTO
            {
                AdminId = admin.AdminId,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                Role = admin.Role,
                IsActive = admin.IsActive,
                LastLoginAt = admin.LastLoginAt,
                CreatedAt = admin.CreatedAt
            }
        };
    }
}