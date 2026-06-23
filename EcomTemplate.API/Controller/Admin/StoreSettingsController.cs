using EcomTemplate.Application.DTOs.AdminDTOs;
using EcomTemplate.Application.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace EcomTemplate.API.Controllers;

[ApiController]
[Route("api/admin/store-settings")]
public class StoreSettingsController : ControllerBase
{
    private readonly IStoreSettings _repo;

    public StoreSettingsController(
        IStoreSettings repo
    )
    {
        _repo = repo;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStoreSettings(
        [FromBody] StoreSettingsDTO dto
    )
    {
        try
        {
            var settings =
                await _repo.CreateStoreSettings(dto);

            return Ok(settings);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = ex.Message
            });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateStoreSettings(
        [FromBody] StoreSettingsDTO dto
    )
    {
        try
        {
            var settings =
                await _repo.UpdateStoreSettings(dto);

            return Ok(settings);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = ex.Message
            });
        }
    }

    [HttpGet]
public async Task<IActionResult> GetStoreSettings()
{
    var settings =
        await _repo.GetStoreSettings();

    if (settings == null)
    {
        return NotFound(new
        {
            Message = "Store settings not found"
        });
    }

    return Ok(settings);
}
}