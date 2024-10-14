using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class History
{
    [BsonElement("disease")]
    public string Disease { get; set; }

    [BsonElement("treatment")]
    public string Treatment { get; set; }

    public History()
    {
        
    }

    public History(string disease, string treatment)
    {
        Disease = disease;
        Treatment = treatment;
    }
}