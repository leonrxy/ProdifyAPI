using Microsoft.AspNetCore.Mvc;
using Prodify.Services;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userSvc;
    public AuthController(IUserService userSvc) => _userSvc = userSvc;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        try
        {
            var token = await _userSvc.AuthenticateAsync(dto.Email, dto.Password);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // Untuk logout, biasanya cukup hapus token di client
        // Di sini kita hanya mengembalikan OK
        return Ok(new { message = "Logout successful" });
    }
}