using GrocerySupermarket.Application.DTOs.AdminDTOs;

namespace GrocerySupermarket.Application.Interfaces.Admin;

public interface IDashboardService
{
    Task<RevenueStatDTO> GetRevenueStatAsync();
    Task<OrdersStatDTO> GetOrdersStatAsync();
    Task<CustomersStatDTO> GetCustomersStatAsync();
    Task<ProductsStatDTO> GetProductsStatAsync();
}