using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Country
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("code")]
    public string Code { get; set; }

    [BsonElement("timezone")]
    public string Timezone { get; set; }
}