using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prodify.Common;
using Prodify.Services;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboard;
    public DashboardController(IDashboardService dashboardService) => _dashboard = dashboardService;

    [HttpGet("total-users-products")]
    public async Task<IActionResult> GetTotalUsers()
    {
        try
        {
            var data = await _dashboard.GetTotalUsersProductsAsync();
            return Ok(ResponseFactory.Success(data, "Total users and products retrieved successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

}