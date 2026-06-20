using EcomTemplate.Application.DTOs.AdminDTOs.AdminDashboardTables;
using EcomTemplate.Domain.Entities;

namespace EcomTemplate.Application.Interfaces.Admin.AdminDashboardTables;


public interface IDashboardOrderRepository
{
    Task<List<DashboardOrderDTO>> GetAllOrders(int pageNumber, int pageSize);
    Task<Order?> GetOrderDetails(Guid orderId);
}