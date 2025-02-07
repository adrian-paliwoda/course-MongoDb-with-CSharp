using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Links
{
    [BsonElement("self")]
    public Self Self { get; set; }

    [BsonElement("previousepisode")]
    public Episode PreviousEpisode { get; set; }
    
    [BsonElement("nextepisode")]
    public Episode NextEpisode { get; set; }
}