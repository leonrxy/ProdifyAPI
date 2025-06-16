using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prodify.Common;
using Prodify.Services;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService authService) => _auth = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        try
        {
            var token = await _auth.AuthenticateAsync(dto.Username, dto.Password);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        // Untuk logout, biasanya cukup hapus token di client
        // Di sini kita hanya mengembalikan OK
        return Ok(new { status = "success", message = "Logout successful" });
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var user = await _auth.GetProfileAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user == null)
                return NotFound(new { error = "User not found" });
            return Ok(ResponseFactory.Success(user, "User profile retrieved successfully"));
        }
        catch (Exception ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}