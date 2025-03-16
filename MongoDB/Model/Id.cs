using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Id
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("value")]
    public string Value { get; set; }
}