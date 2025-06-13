using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;
public class LoginRequestDto
{
    [Description("Email pengguna")]
    [DefaultValue("admin@mail.com")]
    public string Email { get; set; }

    [Description("Password pengguna")]
    [DefaultValue("admin123")]
    public string Password { get; set; }
}