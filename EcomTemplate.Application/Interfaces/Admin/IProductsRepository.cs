using EcomTemplate.Domain.Entities;

namespace EcomTemplate.Application.Interfaces.Admin;


public interface IAddProducts
{
    Task<List<Product>> AddNewProducts(List<Product> products);
    Task<bool> DeleteProduct(Guid productId);
    Task<Product?> UpdateProduct(Product product, Guid productId);
}