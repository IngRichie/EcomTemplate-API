using Microsoft.AspNetCore.Mvc;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Application.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace GrocerySupermarket.WebAPI.Controller;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<ProductController> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    // ===============================
    // PRODUCT DETAILS (FULL INFO)
    // ===============================
    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> ProductDetails(Guid productId)
    {
        var product = await _productRepository.GetProductDetails(productId);

        if (product == null)
            return NotFound();

       // var dto = _mapper.Map<ProductDTO>(product);
        return Ok(product);
    }

    // ===============================
    // PRODUCTS BY CATEGORY (PAGED)
    // ===============================
    // [HttpGet("category-products")]
    // public async Task<IActionResult> ProductsByCategory(
    //     [FromQuery] int page = 1,
    //     [FromQuery] int pageSize = 10)
    // {
    //     var products = await _productRepository
    //         .GetProductsByCategory(page, pageSize);

    //     if (!products.Any())
    //         return Ok(new List<CategoryDTO>());

    //     var dto = _mapper.Map<List<CategoryDTO>>(products);
    //     return Ok(dto);
    // }

    // ===============================
    // PRODUCTS BY CATEGORY ID
    // ===============================
    [HttpGet("category/{categoryId:guid}")]
    public async Task<IActionResult> ProductsByCategoryId(Guid categoryId)
    {
        var category = await _productRepository.GetACategoryWithProducts(categoryId);

        if (category == null)
            return NotFound();

        var dto = _mapper.Map<List<ProductDTO>>(category.Products);
        return Ok(dto);
    }

    // ===============================
    // TOP PRODUCTS (BY REVIEWS)
    // ===============================
    [HttpGet("top")]
    public async Task<IActionResult> TopProducts([FromQuery] int limit = 10)
    {
        var products = await _productRepository.GetTopProducts(limit);

        if (!products.Any())
            return Ok(new List<ProductDTO>());

        var dto = _mapper.Map<List<ProductDTO>>(products);
        return Ok(dto);
    }

    // ===============================
    // NEW PRODUCTS
    // ===============================
  [HttpGet("new")]
public async Task<IActionResult> NewProducts([FromQuery] int limit = 10)
{
    var products = await _productRepository.GetNewProducts(limit);

    if (!products.Any())
        return Ok(new List<ProductDTO>());

    var dto = _mapper.Map<List<ProductDTO>>(products);
 
    return Ok(dto);
}


    // ===============================
    // MOST POPULAR PRODUCTS (BY SALES)
    // ===============================
    [HttpGet("popular-products")]
    public async Task<IActionResult> MostPopularProducts(
        [FromQuery] int limit = 10)
    {
        var products = await _productRepository
            .GetMostPopularProductsAsync(limit);

        if (!products.Any())
            return Ok(new List<ProductDTO>());

        var dto = _mapper.Map<List<ProductDTO>>(products);
        return Ok(dto);
    }
}
