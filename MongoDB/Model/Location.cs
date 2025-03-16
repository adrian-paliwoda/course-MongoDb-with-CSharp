using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class Location
{
    [BsonElement("street")]
    public string Street { get; set; }

    [BsonElement("city")]
    public string City { get; set; }

    [BsonElement("state")]
    public string State { get; set; }

    [BsonElement("postcode")]
    public int Postcode { get; set; }

    [BsonElement("coordinates")]
    public Coordinates Coordinates { get; set; }

    [BsonElement("timezone")]
    public Timezone Timezone { get; set; }
}