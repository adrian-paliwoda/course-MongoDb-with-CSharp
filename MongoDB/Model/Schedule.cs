using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Schedule
{
    [BsonElement("time")]
    public string Time { get; set; }

    [BsonElement("days")]
    public List<string> Days { get; set; }
}