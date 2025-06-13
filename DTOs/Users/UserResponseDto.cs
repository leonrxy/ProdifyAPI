namespace Prodify.Dtos.UserDto;

public class ListDto
{
    public string id { get; set; }
    public required string username { get; set; }
    public string? email { get; set; }
    public string? name { get; set; }
    public string? role { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
}

public class DetailDto
{
    public string id { get; set; }
    public required string username { get; set; }
    public string? email { get; set; }
    public string? name { get; set; }
    public string? role { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }

}

public class SimpleDto
{
    public string? id { get; set; }
    public string? name { get; set; }
    public string? email { get; set; }
}
