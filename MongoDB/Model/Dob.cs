using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Dob
{
    [BsonElement("date")]
    public DateTime Date { get; set; }

    [BsonElement("age")]
    public int Age { get; set; }
}