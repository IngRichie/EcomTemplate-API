using Microsoft.AspNetCore.Mvc;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] CustomerDTO dto,
        [FromQuery] string password)
    {
        var result = await _service.Register(dto, password);
        return Ok(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var result = await _service.Login(dto);
            return Ok(result);
        }
        catch (UnauthorizedException ex)
        {
            return Unauthorized(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromQuery] string refreshToken)
    {
        try
        {
            var result = await _service.RefreshToken(refreshToken);
            return Ok(result);
        }
        catch (UnauthorizedException ex)
        {
            return Unauthorized(new
            {
                success = false,
                message = ex.Message
            });
        }
    }
}
