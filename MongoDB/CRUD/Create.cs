using MongoDB.Driver;
using MongoDB.Model;
using MongoDB.Utils;

namespace MongoDB.CRUD;

public class Create
{
    private readonly IMongoDatabase? _database;

    public Create()
    {
        var server = new MongoClient(MongoDbHelper.GenerateMongoDbSettings());
        _database = server.GetDatabase(DatabaseNames.FlightsDbName);
    }

    public void Example_Create_Flights()
    {
        if (_database != null)
        {
            var collection = _database.GetCollection<FlightData>(DatabaseNames.FlightDataCollectionName);

            var flightData0 = new FlightData("KRK", "LDN", "WIZZAR", 1000, true);
            var flightData1 = new FlightData("LDN", "KRK", "WIZZ", 1000, true);
            var flightData2 = new FlightData("ASA", "NYC", "LOT", 3000, true);

            collection.InsertOne(flightData0);
            collection.InsertMany([flightData1, flightData2], new InsertManyOptions
            {
                IsOrdered = false
            });
            MongoDbHelper.ShowDocuments<FlightData>(_database, DatabaseNames.FlightDataCollectionName);
        }
    }
}