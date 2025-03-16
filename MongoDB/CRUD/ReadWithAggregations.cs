using MongoDB.Driver;
using MongoDB.Utils;

namespace MongoDB.CRUD;

public class ReadWithAggregations
{
    private readonly IMongoDatabase? _database;

    public ReadWithAggregations()
    {
        var server = new MongoClient(DatabaseNames.ConnectionString);
        _database = server.GetDatabase(DatabaseNames.FlightsDbName);
    }
    
    
}