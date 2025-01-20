using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model;

public class FlightData
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId Id { get; set; }
    [BsonElement("departureAirport")]
    public string DepartureAirport { get; set; }
    [BsonElement("arrivalAirport")]
    public string ArrivalAirport { get; set; }
    [BsonElement("aircraft")]
    public string Aircraft { get; set; }
    [BsonElement("distance")]
    public int Distance { get; set; }
    [BsonElement("intercontinental")]
    public bool Intercontinental { get; set; }

    public FlightData()
    {
        
    }

    public FlightData(string departureAirport, string arrivalAirport, string aircraft, int distance, bool intercontinental)
    {
        DepartureAirport = departureAirport;
        ArrivalAirport = arrivalAirport;
        Aircraft = aircraft;
        Distance = distance;
        Intercontinental = intercontinental;
    }

    public override string ToString()
    {
        return @$"{nameof(Id)}:{Id}
{nameof(DepartureAirport)}:{DepartureAirport}
{nameof(ArrivalAirport)}:{ArrivalAirport}
{nameof(Aircraft)}:{Aircraft}
{nameof(Distance)}:{Distance}
{nameof(Intercontinental)}:{Intercontinental}
";
    }
}