

using EcomTemplate.Application.Interfaces.Admin.AdminDashboardTables;
using Microsoft.AspNetCore.Mvc;

namespace EcomTemplate.API.Controller.Admin.AdminDashboardTables;

[ApiController]
[Route("api/admin/dashboard-order")]
public class DashboardOrders : ControllerBase
{
    private readonly IDashboardOrderRepository _dashboardOrderRespo;

    public DashboardOrders(IDashboardOrderRepository dashboardOrderRepository)
    {
        _dashboardOrderRespo = dashboardOrderRepository;
    }

    [HttpGet("/orders")]
    public async Task<IActionResult> GetAllOrders([FromQuery]int pageSize =20, [FromQuery] int pageNumber =2)
    {
        var orders = await _dashboardOrderRespo.GetAllOrders(pageSize, pageNumber);
        if (orders == null)
        {
          return  NotFound(
                new
                {
                    
                    Message = "Orders not found",
                }
            );
        }

      return  StatusCode(
            200,
            new
            {
                message = "Orders found",
                data = orders
            }
        );
    }

    [HttpGet("/orders-details")]
    public async Task<IActionResult> OrderDetails([FromQuery]Guid orderId)
    {
        var orderDetails = await _dashboardOrderRespo.GetOrderDetails(orderId);

        if(orderDetails == null)
        {
            return NotFound(
            
                new
                {
                    message = "Order not found",
                }
            );
        }

        return StatusCode(
            200,
            new
            {
                message = "order details found",
                data = orderDetails
            }
        );
    }
}
