using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Model;

namespace MongoDB.CRUD;

public class Update
{
    private readonly MongoClient _server;
    private const string FlightDataCollectionName = "flightData";
    private const string ConnectionString = "mongodb://localhost:27017";
    const string Flights = "flights";

    public Update()
    {
        _server = new MongoClient(ConnectionString);
    }
    
    public void Example_One()
    {
        var database = _server.GetDatabase(Flights);
        if (database != null)
        {
            var collection = database.GetCollection<FlightData>(FlightDataCollectionName);
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
        var database = _server.GetDatabase(Flights);
        if (database != null)
        {
            var collection = database.GetCollection<FlightData>(FlightDataCollectionName);
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
        var database = _server.GetDatabase(Flights);
        if (database != null)
        {
            var collection = database.GetCollection<FlightData>(FlightDataCollectionName);
            var filterBuilder = new FilterDefinitionBuilder<FlightData>();
            var filter = filterBuilder.Eq(p => p.Aircraft, "WIZZ");
            var updateDefinitionBuilder = new UpdateDefinitionBuilder<FlightData>();
            var update = updateDefinitionBuilder.Set(p => p.Aircraft, "WIZZAR");
            var document = collection.FindOneAndUpdate(filter, update);

            Console.WriteLine(document.ToString());
        }
        
    }
    
    public async Task Example_1()
    {
        var database = _server.GetDatabase("movieData");
        if (database != null)
        {
            var collection = database.GetCollection<Movie>("movies");
            var filter = Builders<Movie>.Filter.Eq(m => m.Id, new ObjectId("678e86bfd845a014f66931e5"));
            
            var result = await collection.FindAsync(filter);
            var document = await result.FirstOrDefaultAsync();

            var update = Builders<Movie>.Update.Set(m => m.Weight, 101);
            var resultUpdate = collection.UpdateMany(filter, update, new UpdateOptions { IsUpsert = true });

            Console.WriteLine(document.Name);
        }
        
    }

}