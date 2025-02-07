using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class User
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId Id { get; set; }
    
    [BsonElement("name")] 
    public string Name { get; set; }
    
    [BsonElement("hobbies")]
    public List<Hobby> Hobbies { get; set; }
}