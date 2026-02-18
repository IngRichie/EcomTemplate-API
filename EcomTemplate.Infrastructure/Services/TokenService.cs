using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GrocerySupermarket.Infrastructure.Security;

public class TokenService
{
    // =====================================================
    // ACCESS TOKEN (JWT)
    // =====================================================
    public string CreateAccessToken(CustomerProfile profile)
    {
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
            ?? throw new InvalidOperationException("JWT_KEY missing");

        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
            ?? throw new InvalidOperationException("JWT_ISSUER missing");

        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
            ?? throw new InvalidOperationException("JWT_AUDIENCE missing");

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        );

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        var claims = new List<Claim>
        {
            // Primary identity
            new Claim(JwtRegisteredClaimNames.Sub, profile.CustomerProfileId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, profile.CustomerProfileId.ToString()),

            // User info
            new Claim(JwtRegisteredClaimNames.Email, profile.Email ?? string.Empty),

            // Token metadata
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64
            )
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(15), // ⏱ short-lived access token
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // =====================================================
    // REFRESH TOKEN
    // =====================================================
    public RefreshToken CreateRefreshToken(
        Guid customerProfileId,
        bool rememberMe = false)
    {
        return new RefreshToken
        {
            CustomerProfileId = customerProfileId,
            Token = Guid.NewGuid().ToString("N"),
            ExpiresAt = rememberMe
                ? DateTime.UtcNow.AddDays(30)
                : DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    // =====================================================
    // SOCIAL LOGIN (STUB – VERIFY WITH PROVIDER LATER)
    // =====================================================
    public Task<SocialUserInfo> VerifySocialToken(SocialLoginDto dto)
    {
        // TODO:
        // - Google: verify id_token via Google APIs
        // - Apple: verify identityToken + public keys

        return Task.FromResult(new SocialUserInfo
        {
            Email = "stub@example.com",
            FirstName = "Social",
            LastName = "User"
        });
    }
}

// =========================================================
// SOCIAL USER RESULT MODEL
// =========================================================
public class SocialUserInfo
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
