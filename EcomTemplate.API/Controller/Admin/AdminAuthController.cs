using GrocerySupermarket.Application.DTOs.AdminDTOs;
using GrocerySupermarket.Application.Interfaces.Admin;
using GrocerySupermarket.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GrocerySupermarket.API.Controllers.Admin;

[ApiController]
[Route("api/admin/auth")]
public class AdminAuthController : ControllerBase
{
    private readonly IAdminAuthService _authService;

    public AdminAuthController(IAdminAuthService authService)
    {
        _authService = authService;
    }

    // =======================
    // REGISTER ADMIN
    // =======================
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AdminRegisterDto dto)
    {
        try
        {
            var result = await _authService.Register(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // =======================
    // LOGIN ADMIN
    // =======================
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AdminLoginDto dto)
    {
        try
        {
            var result = await _authService.Login(dto);
            return Ok(result);
        }
        catch (UnauthorizedException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    // =======================
    // REFRESH TOKEN
    // =======================
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        try
        {
            var result = await _authService.RefreshToken(refreshToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}