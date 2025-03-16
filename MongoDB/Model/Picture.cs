using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Picture
{
    [BsonElement("large")]
    public string Large { get; set; }

    [BsonElement("medium")]
    public string Medium { get; set; }

    [BsonElement("thumbnail")]
    public string Thumbnail { get; set; }
}