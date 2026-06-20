namespace EcomTemplate.Application.DTOs.AdminDTOs.AdminDashboardTables;
public class DashboardOrderDTO
{
    public Guid OrderId { get; set; }

    public string CustomerName { get; set; } = "";

    public string? CustomerEmail { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "";

    public DateTime CreatedAt { get; set; }
}