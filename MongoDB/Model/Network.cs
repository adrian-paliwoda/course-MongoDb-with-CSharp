using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Network
{
    [BsonRepresentation(BsonType.Int32)]
    [BsonElement("id")]
    public int Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("country")]
    public Country Country { get; set; }
}