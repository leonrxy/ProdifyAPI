using System.ComponentModel.DataAnnotations;
using Prodify.Requests;

public class UserPaginatedRequest : PaginatedRequestDto
{
    public string? search { get; set; } = null;
}

public class CreateUserRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string email { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string username { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string name { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string password { get; set; }

    public string role { get; set; }
}

public class UpdateUserRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string email { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string username { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string name { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string password { get; set; }

    public string? role { get; set; }
}
