using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class ExamScore
{
    [BsonElement("difficulty")]
    public int Difficulty { get; set; }

    [BsonElement("score")]
    public double Score { get; set; }
}