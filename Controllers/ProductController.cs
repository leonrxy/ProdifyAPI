using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
[Authorize]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() => Ok(new[] { "Produk A", "Produk B" });

    [HttpPost]
    [Authorize(Roles = "Admin")]  // hanya Admin
    public IActionResult Create([FromBody] string name) 
        => Created("", new { name });
}
