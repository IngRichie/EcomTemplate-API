using AutoMapper;
using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrocerySupermarket.WebAPI.Controller;

[ApiController]
[AllowAnonymous]
[Route("api/banners")]
public class BannersController : ControllerBase
{
    private readonly IBannerRepository _repo;
    private readonly IMapper _mapper;

    public BannersController(IBannerRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveBanners()
    {
        var banners = await _repo.GetActiveAsync();
        var dto = _mapper.Map<List<BannerDTO>>(banners);
        return Ok(dto);
    }
}
