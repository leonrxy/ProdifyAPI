namespace Prodify.Dtos.ProductDto
{
    public class ListDto
    {
        public string id { get; set; }
        public required string name { get; set; }
        public required decimal price { get; set; }
        public required string status { get; set; }
        public required int stock { get; set; }
        public DateTime? display_start { get; set; }
        public DateTime? display_end { get; set; }
    }

    public class DetailDto
    {
        public string id { get; set; }
        public required string name { get; set; }
        public string? description { get; set; }
        public required decimal price { get; set; }
        public string? photo_url { get; set; }
        public DateTime? display_start { get; set; }
        public DateTime? display_end { get; set; }
        public required string status { get; set; }
        public required int stock { get; set; }
        public string? admin_id { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }

    public class SimpleDto
    {
        public string? id { get; set; }
        public string? name { get; set; }
    }
}
