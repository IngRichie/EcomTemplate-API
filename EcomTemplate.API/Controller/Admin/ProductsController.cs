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
public class ProductsController : ControllerBase
{
    private readonly IAddProducts _repo;
    private readonly IMapper _mapper;

    public ProductsController(IAddProducts repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpPost]
public async Task<IActionResult> AddNewProducts(List<ProductDTO> productsDTO)
{
    var products = _mapper.Map<List<Product>>(productsDTO);

    var result = await _repo.AddNewProducts(products); 

    return Ok(result);
}




    
}