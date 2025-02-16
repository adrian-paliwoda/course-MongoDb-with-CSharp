using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Externals
{
    [BsonElement("tvrage")]
    [BsonRepresentation(BsonType.Int32)]
    public int Tvrage { get; set; }

    [BsonElement("thetvdb")]
    [BsonRepresentation(BsonType.Int32)]
    public int Thetvdb { get; set; }

    [BsonElement("imdb")]
    public string Imdb { get; set; }
}