using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Prodify.Models
{
    public class User : BaseModel
    {
        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("role")]
        public string Role { get; set; }

    }
}