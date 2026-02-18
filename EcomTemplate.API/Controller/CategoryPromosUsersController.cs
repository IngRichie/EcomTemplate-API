using AutoMapper;
using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GrocerySupermarket.WebAPI.Controller;

[ApiController]
[Route("api/category-promos")]
public class CategoryPromosController : ControllerBase
{
    private readonly ICategoryPromoRepository _repo;
    private readonly IMapper _mapper;

    public CategoryPromosController(ICategoryPromoRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetActivePromos()
    {
        var promos = await _repo.GetActiveAsync();
        var dto = _mapper.Map<List<CategoryPromoDTO>>(promos);
        return Ok(dto);
    }
}
