using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Previousepisode
{
    [BsonElement("href")]
    public string Href { get; set; }
}