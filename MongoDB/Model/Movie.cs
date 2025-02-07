using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Movie
{
    [BsonId]
    [BsonRepresentation(BsonType.Int32)]
    public int Id { get; set; }

    [BsonElement("url")]
    public string Url { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("type")]
    public string Type { get; set; }

    [BsonElement("language")]
    public string Language { get; set; }

    [BsonElement("genres")]
    public List<string> Genres { get; set; }

    [BsonElement("status")]
    public string Status { get; set; }

    [BsonElement("runtime")]
    public int Runtime { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    [BsonElement("premiered")]
    public System.DateTime Premiered { get; set; }

    [BsonElement("officialSite")]
    public string OfficialSite { get; set; }

    [BsonElement("schedule")]
    public Schedule Schedule { get; set; }

    [BsonElement("rating")]
    public Rating Rating { get; set; }

    [BsonElement("weight")]
    public int Weight { get; set; }

    [BsonElement("network")]
    public Network Network { get; set; }

    [BsonElement("webChannel")]
    public object WebChannel { get; set; }  // Can be null, using object for flexibility

    [BsonElement("externals")]
    public Externals Externals { get; set; }

    [BsonElement("image")]
    public Image Image { get; set; }

    [BsonElement("summary")]
    public string Summary { get; set; }

    [BsonRepresentation(BsonType.Int64)]
    [BsonElement("updated")]
    public long Updated { get; set; }

    [BsonElement("_links")]
    public Links Links { get; set; }
}