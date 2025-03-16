using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Coordinates
{
    [BsonElement("latitude")]
    public string Latitude { get; set; }

    [BsonElement("longitude")]
    public string Longitude { get; set; }
}