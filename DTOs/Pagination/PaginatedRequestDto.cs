namespace Prodify.Requests;

public abstract class PaginatedRequestDto
{
    public int page_number { get; set; } = 1;
    public int page_size { get; set; } = 10;
}
