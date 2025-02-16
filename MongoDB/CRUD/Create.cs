using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Model;
using MongoDB.Utils;

namespace MongoDB.CRUD;

public class Create
{
    private readonly MongoClient _server;
    private readonly IMongoDatabase? _database;
    private const string FlightDataCollectionName = "flightData";
    private const string ConnectionString = "mongodb://localhost:27017";
    const string Flights = "flights";

    public Create()
    {
        _server = new MongoClient(ConnectionString);
        _database = _server.GetDatabase(Flights);
    }
    
    public void Example_0()
    {
        if (_database != null)
        {
            var collection = _database.GetCollection<FlightData>(FlightDataCollectionName);

            var flightData0 = new FlightData("KRK", "LDN", "WIZZAR", 1000, true);
            
            var flightData1 = new FlightData("LDN", "KRK", "WIZZ", 1000, true);
            var flightData2 = new FlightData("ASA", "NYC", "LOT", 3000, true);

            collection.InsertOne(flightData0);
            collection.InsertMany(new []{flightData1, flightData2}, new InsertManyOptions()
            {
                IsOrdered = false
            });
        }
        MongoDbHelper.ShowDocuments<FlightData>(_database, FlightDataCollectionName);
    }
    
    public void Example_1_ReadFromFile()
    {
        if (_database != null)
        {
            var collection = _database.GetCollection<FlightData>(FlightDataCollectionName);

            var pathToFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "flightDataExample.json");
            var bsonFromFile = File.ReadAllText(pathToFile);
            var bsonDocuments = BsonSerializer.Deserialize<IList<FlightData>>(bsonFromFile);
            
            
            collection.InsertMany(bsonDocuments);
        }
        
        MongoDbHelper.ShowDocuments<FlightData>(_database, FlightDataCollectionName);
    }
}