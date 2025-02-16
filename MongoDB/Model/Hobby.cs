using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Hobby
{
    [BsonElement("title")]
    public string Title { get; set; }
    
    [BsonElement("frequency")]
    public int Frequency { get; set; }
    
    [BsonElement("feq")]
    public int Feqy { get; set; }
    
    [BsonExtraElements]
    public BsonDocument CatchAll { get; set; }
}