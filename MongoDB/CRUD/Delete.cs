using MongoDB.Driver;
using MongoDB.Model;

namespace MongoDB.CRUD;

public class Delete
{
    private readonly MongoClient _server;
    private readonly IMongoDatabase? _database;
    private const string FlightDataCollectionName = "flightData";
    private const string ConnectionString = "mongodb://localhost:27017";
    const string Flights = "flights";

    public Delete()
    {
        _server = new MongoClient(ConnectionString);
        _database = _server.GetDatabase(Flights);
    }

    public void ExampleOne()
    {
        if (_database != null)
        {
            var collection = _database.GetCollection<FlightData>(FlightDataCollectionName);
            var filterBuilder = new FilterDefinitionBuilder<FlightData>();
            var filter = filterBuilder.Eq(p => p.Aircraft, "WIZZ");

            var deleteResult = collection.DeleteOne(filter);

            Console.WriteLine(deleteResult.DeletedCount);
        }
    }

    public void ExampleMany()
    {
        if (_database != null)
        {
            var collection = _database.GetCollection<FlightData>(FlightDataCollectionName);
            var filterBuilder = new FilterDefinitionBuilder<FlightData>();
            var filter = filterBuilder.Eq(p => p.Aircraft, "WIZZ");

            var deleteResult = collection.DeleteOne(filter);

            Console.WriteLine(deleteResult.DeletedCount);
        }
    }
}