using AutoMapper;
using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrocerySupermarket.WebAPI.Controller.Admin;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/banners")]
public class AdminBannersController : ControllerBase
{
    private readonly IBannerRepository _repo;
    private readonly IMapper _mapper;

    public AdminBannersController(IBannerRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create(BannerDTO dto)
    {
        var banner = _mapper.Map<Banner>(dto);
        await _repo.AddAsync(banner);
        await _repo.SaveAsync();
        return Ok(_mapper.Map<BannerDTO>(banner));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, BannerDTO dto)
    {
        var banner = await _repo.GetByIdAsync(id);
        if (banner == null) return NotFound();

        _mapper.Map(dto, banner);
        await _repo.UpdateAsync(banner);
        await _repo.SaveAsync();

        return Ok(_mapper.Map<BannerDTO>(banner));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var banner = await _repo.GetByIdAsync(id);
        if (banner == null) return NotFound();

        await _repo.DeleteAsync(banner);
        await _repo.SaveAsync();

        return NoContent();
    }
}
