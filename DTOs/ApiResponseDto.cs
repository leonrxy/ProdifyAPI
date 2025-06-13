using System.Text.Json.Serialization;

public class ApiResponse<T>
{
    public string Status { get; set; }   // misal "success" / "error"
    public string Message { get; set; }   // deskripsi singkat
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; set; }   // payload data
}
