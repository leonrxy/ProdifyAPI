using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Prodify.Models
{
    public enum ProductStatus
    {
        draft,
        active,
        inactive,
        delete
    }

    public class AdminInfo
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }
    }

    public class Product : BaseModel
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("admin")]
        public AdminInfo Admin { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        // Harga (validasi di DTO/service)
        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("photo_url")]
        public string PhotoUrl { get; set; }

        [BsonElement("display_start")]
        public DateTime DisplayStart { get; set; }

        [BsonElement("display_end")]
        public DateTime DisplayEnd { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("status")]
        public ProductStatus Status { get; set; }

        [BsonElement("stock")]
        public int Stock { get; set; }
    }
}
