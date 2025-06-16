using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;
public class LoginRequestDto
{
    [Description("Username atau Email")]
    [DefaultValue("admin@mail.com")]
    public string Username { get; set; }

    [Description("Password pengguna")]
    [DefaultValue("admin123")]
    public string Password { get; set; }
}