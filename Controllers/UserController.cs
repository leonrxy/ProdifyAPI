using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prodify.Common;
using Prodify.Dtos;
using Prodify.Services;

namespace Prodify.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "superadmin, admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginated([FromQuery] UserPaginatedRequest request)
        {
            var result = await _service.GetPaginatedAsync(request);
            return Ok(ResponseFactory.Success(result, "Users Retrieved Successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _service.GetByIdAsync(id);
                return Ok(ResponseFactory.Success(user, "User retrieved successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ResponseFactory.Error(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequestDto request)
        {
            try
            {
                await _service.CreateAsync(request);
                return Ok(ResponseFactory.Success("User created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseFactory.Error(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserRequestDto request)
        {
            try
            {
                await _service.UpdateAsync(id, request);
                return Ok(ResponseFactory.Success("User updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ResponseFactory.Error(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(ResponseFactory.Success("User deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ResponseFactory.Error(ex.Message));
            }
        }
    }
}
