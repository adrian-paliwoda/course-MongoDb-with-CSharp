using MongoDB.Driver;
using MongoDB.Model;

namespace MongoDB.CRUD;

public class Read
{
    private readonly MongoClient _server;
    private readonly IMongoDatabase? _database;
    private const string FlightDataCollectionName = "flightData";
    private const string ConnectionString = "mongodb://localhost:27017";
    const string Flights = "flights";

    public Read()
    {
        _server = new MongoClient(ConnectionString);
        _database = _server.GetDatabase(Flights);
    }
    
    public void Read_Example()
    {
        if (_database != null)
        {
            var collection = _database.GetCollection<FlightData>(FlightDataCollectionName);
            var documents = collection.FindAsync(FilterDefinition<FlightData>.Empty).Result.ToList();

            Console.WriteLine($"In mongoDb in collection {FlightDataCollectionName} there are documents:");
            for (int i = 0; i < documents.Count; i++)
            {
                Console.WriteLine(documents[i]?.ToString());
                Console.WriteLine();
            }
        }
    }
    
    public void Read_Example_1()
    {
        var collection = _database.GetCollection<FlightData>(FlightDataCollectionName);

        var document = new FlightData("AU", "KRK", "Rayanir", 1222, true);

        var filterDefinitionBuilder = new FilterDefinitionBuilder<FlightData>();
        var filter = filterDefinitionBuilder.Eq(p => p.DepartureAirport, document.DepartureAirport)
                     & filterDefinitionBuilder.Eq(p => p.ArrivalAirport, document.ArrivalAirport)
                     & filterDefinitionBuilder.Eq(p => p.Aircraft, document.Aircraft)
                     & filterDefinitionBuilder.Eq(p => p.Distance, document.Distance)
                     & filterDefinitionBuilder.Eq(p => p.Intercontinental, document.Intercontinental);

        var exists = collection.FindAsync(filter).Result.Any(); // By value
        if (exists)
        {
            Console.WriteLine("Document has been found");
        }
        else
        {
            Console.WriteLine("There is no such document in the collection");
        }
        
    }

}