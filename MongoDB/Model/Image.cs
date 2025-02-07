using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Image
{
    [BsonElement("medium")]
    public string Medium { get; set; }

    [BsonElement("original")]
    public string Original { get; set; }
}