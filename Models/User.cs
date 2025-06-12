using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Prodify.Models
{

    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

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

        [BsonElement("created_at")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime created_at { get; set; }

        [BsonElement("updated_at")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime updated_at { get; set; }

        [BsonElement("deleted_at")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? deleted_at { get; set; }

    }
}