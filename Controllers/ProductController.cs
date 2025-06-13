using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prodify.Common;
using Prodify.Dtos;
using Prodify.Services;

namespace Prodify.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginated([FromQuery] ProductPaginatedRequest request)
        {
            var result = await _service.GetPaginatedAsync(request);
            return Ok(ResponseFactory.Success(result, "Products Retrieved Successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var product = await _service.GetByIdAsync(id);
                return Ok(ResponseFactory.Success(product, "Product retrieved successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ResponseFactory.Error(ex.Message));
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "superadmin, admin")]
        public async Task<IActionResult> Create([FromForm] CreateProductRequestDto request)
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _service.CreateAsync(request, adminId);
                return Ok(ResponseFactory.Success("Product created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseFactory.Error(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "superadmin, admin")]
        public async Task<IActionResult> Update(string id, [FromForm] UpdateProductRequestDto request)
        {
            try
            {
                await _service.UpdateAsync(id, request);
                return Ok(ResponseFactory.Success("Product updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ResponseFactory.Error(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "superadmin, admin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(ResponseFactory.Success("Product deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ResponseFactory.Error(ex.Message));
            }
        }
    }
}
