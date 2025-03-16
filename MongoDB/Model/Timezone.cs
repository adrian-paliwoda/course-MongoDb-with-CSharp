using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Timezone
{
    [BsonElement("offset")]
    public string Offset { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }
}