using EcomTemplate.Application.DTOs.AdminDTOs.AdminDashboardTables;
using EcomTemplate.Domain.Entities;

namespace EcomTemplate.Application.Interfaces.Admin.AdminDashboardTables;


public interface IDashboardCustomerRepository
{
    Task<List<DashboardCustomerDTO>> GetAllCustomers(int pageNumber, int pageSize);
    Task<DashboardCustomerDTO?> GetCustomerDetails(Guid customerId);
}