using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prodify.Models;

public abstract class BaseModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string id { get; set; }
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
