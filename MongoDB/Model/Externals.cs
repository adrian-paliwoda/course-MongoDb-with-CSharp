using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Externals
{
    [BsonRepresentation(BsonType.Int32)]
    [BsonElement("tvrage")]
    public int Tvrage { get; set; }

    [BsonRepresentation(BsonType.Int32)]
    [BsonElement("thetvdb")]
    public int Thetvdb { get; set; }

    [BsonElement("imdb")]
    public string Imdb { get; set; }
}