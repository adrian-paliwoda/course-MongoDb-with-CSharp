using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Product
{
    public ObjectId _id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("price")]
    public double Price { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("good")]
    public string Good { get; set; }
    
    [BsonElement("details")]
    public Details Details { get; set; }

    public Product()
    {
        
    }

    public Product(string name, int price, string description, Details details)
    {
        Name = name;
        Price = price;
        Description = description;
        Details = details;
    }
}