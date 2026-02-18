using Microsoft.AspNetCore.Mvc;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Application.DTOs;
using AutoMapper;

namespace GrocerySupermarket.WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryController(
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Home screen categories with limited products
    /// </summary>
    // [HttpGet("with-products")]
    // public async Task<IActionResult> GetCategoriesWithProducts(
    //     [FromQuery] int categoryLimit = 6,
    //     [FromQuery] int productsPerCategory = 6)
    // {
    //     var categories = await _categoryRepository
    //         .GetCategoriesWithProductsAsync(categoryLimit, productsPerCategory);

    //     if (categories.Count == 0)
    //         return NotFound("No categories found.");

    //     var dto = _mapper.Map<List<CategoryDTO>>(categories);
    //     return Ok(dto);
    // }

    [HttpGet("product-categories")]
public async Task<IActionResult> GetCategoriesWithAllProducts(
    [FromQuery] int categoryLimit = 10,
    [FromQuery] int productsPerCategory = 10)
{
    var categories = await _categoryRepository
        .GetAllCategoriesAsync(categoryLimit, productsPerCategory);

    if (!categories.Any())
        return NotFound("No categories found");

    return Ok(categories);
}

}
