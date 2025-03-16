using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Name
{
    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("first")]
    public string First { get; set; }

    [BsonElement("last")]
    public string Last { get; set; }
}