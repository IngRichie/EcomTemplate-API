using AutoMapper;
using EcomTemplate.Application.DTOs;
using EcomTemplate.Application.Interfaces.Admin;
using EcomTemplate.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomTemplate.API.Controllers;

[ApiController]
[Route("api/admin/products/add-new-products")]
[Authorize(Roles = "Admin")] 
public class NewProductsController : ControllerBase
{
    private readonly IAddProducts _repo;
    private readonly IMapper _mapper;

    public NewProductsController(IAddProducts repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

[HttpPost]
public async Task<IActionResult> AddNewProducts([FromBody] List<ProductDTO> productsDTO)
{
    var products = _mapper.Map<List<Product>>(productsDTO);

    var result = await _repo.AddNewProducts(products);

    var response = _mapper.Map<List<ProductDTO>>(result);

    return Ok(response);
}
    
}