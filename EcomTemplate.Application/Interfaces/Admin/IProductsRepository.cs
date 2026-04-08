using EcomTemplate.Domain.Entities;

namespace EcomTemplate.Application.Interfaces.Admin;


public interface IAddProducts
{
    Task<List<Product>> AddNewProducts(List<Product> products);
}