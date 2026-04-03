namespace EcomTemplate.Application.Interfaces.Admin;

public interface IDashboardRespository
{
    Task<double> GetRevenueAsync(DateTime startDate, DateTime endDate);
    Task<int> GetOrdersStatAsync(DateTime startDate, DateTime endDate);
    Task<int> GetCustomersStatAsync(DateTime startDate, DateTime endDate);
    Task<int> GetProductsStatAsync(DateTime startDate, DateTime endDate);
}