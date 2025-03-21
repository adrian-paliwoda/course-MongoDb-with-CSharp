using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Friend
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnore]
    public ObjectId Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("hobbies")]
    public BsonArray Hobbies { get; set; }

    [BsonElement("age")]
    public int Age { get; set; }
    
    [BsonElement("examScores")]
    public List<ExamScore> ExamScores { get; set; }
    
    [BsonExtraElements]
    public BsonDocument ExtraElements { get; set; }   
}