using GrocerySupermarket.Application.DTOs.AdminDTOs;
using GrocerySupermarket.Application.Interfaces.Admin;
using EcomTemplate.API.HelperFunctions;

namespace  GrocerySupermarket.Application.Interfaces.Service.Admin;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRespository _repo;

    public DashboardService(IDashboardRespository repo)
    {
        _repo = repo;
    }

    // =========================
    // REVENUE
    // =========================
    public async Task<RevenueStatDTO> GetRevenueStatAsync()
    {
        var (start, end) = DateTimeHelperFunc.GetMonthRange(0);
        var (lastStart, lastEnd) = DateTimeHelperFunc.GetMonthRange(-1);

        var current = await _repo.GetRevenueAsync(start, end);
        var last = await _repo.GetRevenueAsync(lastStart, lastEnd);

        return new RevenueStatDTO
        {
            TotalRevenue = current,
            RevenueComparison = CalculateComparison(current, last)
        };
    }

    // =========================
    // ORDERS
    // =========================
    public async Task<OrdersStatDTO> GetOrdersStatAsync()
    {
        var (start, end) = DateTimeHelperFunc.GetMonthRange(0);
        var (lastStart, lastEnd) = DateTimeHelperFunc.GetMonthRange(-1);

        var current = await _repo.GetOrdersStatAsync(start, end);
        var last = await _repo.GetOrdersStatAsync(lastStart, lastEnd);

        return new OrdersStatDTO
        {
            TotalOrders = current,
            OrdersComparison = CalculateComparison(current, last)
        };
    }

    // =========================
    // CUSTOMERS
    // =========================
    public async Task<CustomersStatDTO> GetCustomersStatAsync()
    {
        var (start, end) = DateTimeHelperFunc.GetMonthRange(0);
        var (lastStart, lastEnd) = DateTimeHelperFunc.GetMonthRange(-1);

        var current = await _repo.GetCustomersStatAsync(start, end);
        var last = await _repo.GetCustomersStatAsync(lastStart, lastEnd);

        return new CustomersStatDTO
        {
            TotalCustomers = current,
            TotalCustomersComparison = CalculateComparison(current, last)
        };
    }

    // =========================
    // PRODUCTS
    // =========================
    public async Task<ProductsStatDTO> GetProductsStatAsync()
    {
        var (start, end) = DateTimeHelperFunc.GetMonthRange(0);
        var (lastStart, lastEnd) = DateTimeHelperFunc.GetMonthRange(-1);

        var current = await _repo.GetProductsStatAsync(start, end);
        var last = await _repo.GetProductsStatAsync(lastStart, lastEnd);

        return new ProductsStatDTO
        {
            TotalProducts = current,
            TotalProductsComparison = CalculateComparison(current, last)
        };
    }

    // =========================
    // SHARED LOGIC
    // =========================
    private double CalculateComparison(double current, double previous)
    {
        if (previous > 0)
        {
            return ((current - previous) / previous) * 100;
        }

        return current > 0 ? 100 : 0;
    }
}