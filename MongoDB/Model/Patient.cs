using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Patient
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId Id { get; set; }
    
    [BsonElement("firstName")]
    public string FirstName { get; set; }
    
    [BsonElement("lastName")]
    public string LastName { get; set; }
    
    [BsonElement("age")]
    public int Age { get; set; }
    
    [BsonElement("history")]
    public History[] History { get; set; }

    public Patient()
    {
        
    }

    public Patient(string firstName, string lastName, int age, History[] history)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        History = history;
    }
}