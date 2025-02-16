using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Rating
{
    [BsonElement("average")]
    public double? Average { get; set; }
}