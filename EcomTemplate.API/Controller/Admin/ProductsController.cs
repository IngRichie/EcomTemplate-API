using AutoMapper;
using EcomTemplate.Application.DTOs;
using EcomTemplate.Application.Interfaces.Admin;
using EcomTemplate.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomTemplate.API.Controllers;

[ApiController]
[Route("api/admin/products/add-new-products")]
// [Authorize(Roles = "Admin")] 
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
    
[HttpDelete("{productId:guid}")]
public async Task<IActionResult> DeleteProduct(Guid productId)
{
var deleted = await _repo.DeleteProduct(productId);


if (!deleted)
{
    return NotFound(new
    {
        Message = "Product not found"
    });
}

return Ok(new
{
    Message = "Product deleted successfully"
});


}

[HttpPut("{productId:guid}")]
public async Task<IActionResult> UpdateProduct(
Guid productId,
[FromBody] ProductDTO productDto)
{
var product = _mapper.Map<Product>(productDto);


var updatedProduct =
    await _repo.UpdateProduct(product, productId);

if (updatedProduct == null)
{
    return NotFound(new
    {
        Message = "Product not found"
    });
}

var response =
    _mapper.Map<ProductDTO>(updatedProduct);

return Ok(response);


}


}