namespace EcomTemplate.Application.DTOs.AdminDTOs.AdminDashboardTables;

public class DashboardCustomerDTO
{
    public Guid CustomerId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int TotalOrders { get; set; }

    public decimal TotalSpent { get; set; }

    public DateTime JoinedDate { get; set; }

    public string Status { get; set; } = "Active";
}