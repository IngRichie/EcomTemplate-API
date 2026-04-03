using EcomTemplate.Application.DTOs.AdminDTOs;

namespace EcomTemplate.Application.Interfaces.Admin;

public interface IDashboardService
{
    Task<RevenueStatDTO> GetRevenueStatAsync();
    Task<OrdersStatDTO> GetOrdersStatAsync();
    Task<CustomersStatDTO> GetCustomersStatAsync();
    Task<ProductsStatDTO> GetProductsStatAsync();
}