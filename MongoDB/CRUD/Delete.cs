using MongoDB.Driver;
using MongoDB.Model;
using MongoDB.Utils;

namespace MongoDB.CRUD;

public class Delete
{
    private readonly IMongoDatabase? _database;

    public Delete()
    {
        var server = new MongoClient(MongoDbHelper.GenerateMongoDbSettings());
        _database = server.GetDatabase(DatabaseNames.FlightsDbName);
    }

    public void Example_DeleteOneFlight()
    {
        if (_database != null)
        {
            var collection = _database.GetCollection<FlightData>(DatabaseNames.FlightDataCollectionName);
            var filterBuilder = new FilterDefinitionBuilder<FlightData>();
            var filter = filterBuilder.Eq(p => p.Aircraft, DatabaseNames.Wizz);

            var deleteResult = collection.DeleteOne(filter);

            Console.WriteLine(deleteResult.DeletedCount);
        }
    }

    public void Example_DeleteManyFlights()
    {
        if (_database != null)
        {
            var collection = _database.GetCollection<FlightData>(DatabaseNames.FlightDataCollectionName);
            var filterBuilder = new FilterDefinitionBuilder<FlightData>();
            var filter = filterBuilder.Eq(p => p.Aircraft, DatabaseNames.Wizz);

            var deleteResult = collection.DeleteOne(filter);

            Console.WriteLine(deleteResult.DeletedCount);
        }
    }
}