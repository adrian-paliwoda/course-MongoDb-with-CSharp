using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class BSonDocumentNotReal
{
    [BsonElement("_id")]
    public ObjectId Id { get; set; }
    
}