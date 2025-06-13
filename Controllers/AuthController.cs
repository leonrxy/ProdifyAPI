using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var token = await _auth.AuthenticateAsync(dto.Email, dto.Password);
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
}