using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Links
{
    [BsonElement("self")]
    public Self Self { get; set; }

    [BsonElement("previousepisode")]
    public Previousepisode Previousepisode { get; set; }
}