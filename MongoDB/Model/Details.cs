using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Details
{
    public ObjectId _id { get; set; }
    
    [BsonElement("cup")]
    public string Cpu { get; set; }
}