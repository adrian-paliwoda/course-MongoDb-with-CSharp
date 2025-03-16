using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Person
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("gender")]
    public string Gender { get; set; }

    [BsonElement("name")]
    public Name Name { get; set; }

    [BsonElement("location")]
    public Location Location { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("login")]
    public Login Login { get; set; }

    [BsonElement("dob")]
    public Dob Dob { get; set; }

    [BsonElement("registered")]
    public Dob Registered { get; set; }

    [BsonElement("phone")]
    public string Phone { get; set; }

    [BsonElement("cell")]
    public string Cell { get; set; }

    [BsonElement("id")]
    public Id IdDocument { get; set; }

    [BsonElement("picture")]
    public Picture Picture { get; set; }

    [BsonElement("nat")]
    public string Nat { get; set; }
}