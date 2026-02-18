using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using GrocerySupermarket.Infrastructure.Exceptions;
using GrocerySupermarket.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GrocerySupermarket.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly TokenService _tokenService;
    private readonly PasswordHasher<CustomerAuth> _passwordHasher;

    public AuthService(AppDbContext db, TokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
        _passwordHasher = new PasswordHasher<CustomerAuth>();
    }

    // =======================
    // REGISTER (EMAIL/PASSWORD)
    // =======================
    public async Task<AuthResponseDto> Register(CustomerDTO dto, string password)
    {
        var existingProfile = await _db.CustomerProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == dto.Email);

        if (existingProfile != null)
            throw new Exception("Email already registered.");

        var profile = new CustomerProfile
        {
            FirstName = dto.FirstName,
            LastName  = dto.LastName,
            Email     = dto.Email,
            Phone     = dto.Phone,
            CreatedAt = DateTime.UtcNow
        };

        _db.CustomerProfiles.Add(profile);
        await _db.SaveChangesAsync();

        var auth = new CustomerAuth
        {
            CustomerProfileId = profile.CustomerProfileId
        };

        auth.PasswordHash = _passwordHasher.HashPassword(auth, password);

        _db.CustomerAuths.Add(auth);
        await _db.SaveChangesAsync();

        return await GenerateAuthResponse(profile, rememberMe: false);
    }

    // =======================
    // LOGIN (EMAIL/PASSWORD)
    // =======================
    public async Task<AuthResponseDto> Login(LoginDto dto)
    {
        var profile = await _db.CustomerProfiles
            .FirstOrDefaultAsync(x => x.Email == dto.Email) ?? throw new UnauthorizedException("Invalid email or password.");
            
        var auth = await _db.CustomerAuths
            .FirstOrDefaultAsync(x => x.CustomerProfileId == profile.CustomerProfileId) ?? throw new UnauthorizedException("Invalid email or password.");
            
        var verificationResult = _passwordHasher.VerifyHashedPassword(
            auth,
            auth.PasswordHash,
            dto.Password
        );

        if (verificationResult == PasswordVerificationResult.Failed)
    throw new UnauthorizedException("Invalid email or password.");

        return await GenerateAuthResponse(profile, dto.RememberMe);
    }

    // =======================
    // SOCIAL LOGIN (GOOGLE / APPLE)
    // =======================
    public async Task<AuthResponseDto> SocialLogin(SocialLoginDto dto)
    {
        var socialUser = await _tokenService.VerifySocialToken(dto);

        var profile = await _db.CustomerProfiles
            .FirstOrDefaultAsync(x => x.Email == socialUser.Email);

        if (profile == null)
        {
            profile = new CustomerProfile
            {
                Email = socialUser.Email,
                FirstName = socialUser.FirstName,
                LastName = socialUser.LastName,
                CreatedAt = DateTime.UtcNow
            };

            _db.CustomerProfiles.Add(profile);
            await _db.SaveChangesAsync();
        }

        return await GenerateAuthResponse(profile, dto.RememberMe);
    }

    // =======================
    // REFRESH TOKEN
    // =======================
    public async Task<AuthResponseDto> RefreshToken(string refreshToken)
    {
        var token = await _db.RefreshTokens
            .Include(x => x.CustomerProfile)
            .FirstOrDefaultAsync(x => x.Token == refreshToken);

        if (token == null || token.IsRevoked || token.ExpiresAt < DateTime.UtcNow)
            throw new Exception("Invalid refresh token.");

        token.IsRevoked = true;
        await _db.SaveChangesAsync();

        return await GenerateAuthResponse(token.CustomerProfile, rememberMe: false);
    }

    // =======================
    // INTERNAL HELPERS
    // =======================
    private async Task<AuthResponseDto> GenerateAuthResponse(
        CustomerProfile profile,
        bool rememberMe)
    {
        var accessToken = _tokenService.CreateAccessToken(profile);
        var refreshToken = _tokenService.CreateRefreshToken(profile.CustomerProfileId, rememberMe);

        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync();

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = 3600, // 1 hour (stub)
            Customer = new CustomerDTO
            {
                CustomerId = profile.CustomerProfileId,
                FirstName = profile.FirstName ?? string.Empty,
                LastName = profile.LastName ?? string.Empty,
                Email = profile.Email ?? string.Empty,
                Phone = profile.Phone ?? string.Empty,
                CreatedAt = profile.CreatedAt
            }
        };
    }
}
