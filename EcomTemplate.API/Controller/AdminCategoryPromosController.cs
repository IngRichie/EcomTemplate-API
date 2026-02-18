using AutoMapper;
using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrocerySupermarket.WebAPI.Controller.Admin;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/category-promos")]
public class AdminCategoryPromosController : ControllerBase
{
    private readonly ICategoryPromoRepository _repo;
    private readonly IMapper _mapper;

    public AdminCategoryPromosController(ICategoryPromoRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryPromoDTO dto)
    {
        var promo = _mapper.Map<CategoryPromo>(dto);
        await _repo.AddAsync(promo);
        await _repo.SaveAsync();
        return Ok(_mapper.Map<CategoryPromoDTO>(promo));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CategoryPromoDTO dto)
    {
        var promo = await _repo.GetByIdAsync(id);
        if (promo == null) return NotFound();

        _mapper.Map(dto, promo);
        await _repo.UpdateAsync(promo);
        await _repo.SaveAsync();

        return Ok(_mapper.Map<CategoryPromoDTO>(promo));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var promo = await _repo.GetByIdAsync(id);
        if (promo == null) return NotFound();

        await _repo.DeleteAsync(promo);
        await _repo.SaveAsync();

        return NoContent();
    }
}
