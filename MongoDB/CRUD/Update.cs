using MongoDB.Driver;
using MongoDB.Model;

namespace MongoDB.CRUD;

public class Update
{
    private readonly MongoClient _server;
    private readonly IMongoDatabase? _database;
    private const string FlightDataCollectionName = "flightData";
    private const string ConnectionString = "mongodb://localhost:27017";
    const string Flights = "flights";

    public Update()
    {
        _server = new MongoClient(ConnectionString);
        _database = _server.GetDatabase(Flights);
    }
    
    public void Example_One()
    {
        if (_database != null)
        {
            var collection = _database.GetCollection<FlightData>(FlightDataCollectionName);
            var filterBuilder = new FilterDefinitionBuilder<FlightData>();
            var filter = filterBuilder.Eq(p => p.Aircraft, "WIZZAR");
            var updateDefinitionBuilder = new UpdateDefinitionBuilder<FlightData>();
            var update = updateDefinitionBuilder.Set(p => p.Aircraft, "WIZZ");


            var updateResult = collection.UpdateOne(filter, update);

            Console.WriteLine(updateResult.ModifiedCount);
        }
        
    }
    public void Example_Many()
    {
        if (_database != null)
        {
            var collection = _database.GetCollection<FlightData>(FlightDataCollectionName);
            var filterBuilder = new FilterDefinitionBuilder<FlightData>();
            var filter = filterBuilder.Eq(p => p.Aircraft, "WIZZAR");
            var updateDefinitionBuilder = new UpdateDefinitionBuilder<FlightData>();
            var update = updateDefinitionBuilder.Set(p => p.Aircraft, "WIZZ");


            var updateResult = collection.UpdateMany(filter, update);

            Console.WriteLine(updateResult.ModifiedCount);
        }
        
    }
    public void Example_0()
    {
        if (_database != null)
        {
            var collection = _database.GetCollection<FlightData>(FlightDataCollectionName);
            var filterBuilder = new FilterDefinitionBuilder<FlightData>();
            var filter = filterBuilder.Eq(p => p.Aircraft, "WIZZ");
            var updateDefinitionBuilder = new UpdateDefinitionBuilder<FlightData>();
            var update = updateDefinitionBuilder.Set(p => p.Aircraft, "WIZZAR");
            var document = collection.FindOneAndUpdate(filter, update);

            Console.WriteLine(document.ToString());
        }
        
    }

}