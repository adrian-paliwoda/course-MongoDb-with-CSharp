using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Passenger
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("age")]
    public int Age { get; set; }

    public Passenger()
    {

    }

    public Passenger(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public override string ToString()
    {
        return @$"{nameof(Id)}:{Id}
{nameof(Name)}:{Name}
{nameof(Age)}:{Age}";
    }
}