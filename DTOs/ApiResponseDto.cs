public class ApiResponse<T>
{
    public string Status { get; set; }   // misal "success" / "error"
    public string Message { get; set; }   // deskripsi singkat
    public T Data { get; set; }   // payload data
}
