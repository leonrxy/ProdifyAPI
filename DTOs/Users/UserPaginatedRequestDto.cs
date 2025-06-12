using Prodify.Requests;

public class UserPaginatedRequest : PaginatedRequestDto
{
    public string? search { get; set; } = null;
}