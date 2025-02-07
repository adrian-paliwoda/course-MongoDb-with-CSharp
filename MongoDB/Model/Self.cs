using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Self
{
    [BsonElement("href")]
    public string Href { get; set; }
}