using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Episode
{
    [BsonElement("href")]
    public string Href { get; set; }
}