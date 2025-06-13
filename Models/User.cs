using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Prodify.Models
{

    public class User : BaseModel
    {
        [BsonElement("email")]
        public string email { get; set; }

        [BsonElement("username")]
        public string username { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("password")]
        public string password { get; set; }

        [BsonElement("role")]
        public string role { get; set; }

    }
}