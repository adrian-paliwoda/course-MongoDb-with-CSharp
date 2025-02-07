using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Model;

namespace MongoDB.CRUD;

public class Read
{
    private readonly MongoClient _server;
    public const string FlightDataCollectionName = "flightData";
    public const string MovieDataDBName = "movieData";
    private const string ConnectionString = "mongodb://localhost:27017";
    public const string Flights = "flights";
    public const string MovieCollection = "movies";

    public Read()
    {
        _server = new MongoClient(ConnectionString);
    }
    
    public void Read_Example(string databaseName = Flights)
    {
        var database = _server.GetDatabase(databaseName);
        if (database != null)
        {
            var collection = database.GetCollection<FlightData>(FlightDataCollectionName);
            var documents = collection.FindAsync(FilterDefinition<FlightData>.Empty).Result.ToList();

            Console.WriteLine($"In mongoDb in collection {FlightDataCollectionName} there are documents:");
            for (int i = 0; i < documents.Count; i++)
            {
                Console.WriteLine(documents[i]?.ToString());
                Console.WriteLine();
            }
        }
    }
    
    public void Read_Example_1(string databaseName = Flights, string collectionName = FlightDataCollectionName)
    {
        var database = _server.GetDatabase(databaseName);
        var collection = database.GetCollection<FlightData>(collectionName);

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
    
    public async Task Read_Example_2(string databaseName = Flights, string collectionName = FlightDataCollectionName)
    {
        var database = _server.GetDatabase(databaseName);
        var collection = database.GetCollection<BsonDocument>(collectionName);
        
        var filterDefinitionBuilder = new FilterDefinitionBuilder<BsonDocument>();
        var filter = Builders<BsonDocument>.Filter.ElemMatch("ratings", 
            Builders<BsonValue>.Filter.And(
                Builders<BsonValue>.Filter.Gt("ratings", 8), 
                Builders<BsonValue>.Filter.Lt("ratings", 10)
            )
        );
        // var filter2 = Builders<BsonDocument>.Filter.And(
        //     Builders<BsonDocument>.Filter.Eq(x => x["EntityId"], 1),
        //     Builders<BsonDocument>.Filter.Gte(x => x["ActivityDate"], "adsf"),
        //     Builders<BsonDocument>.Filter.Lte(x => x["ActivityDate"], "requestDetails.ToDate"),
        //     Builders<BsonDocument>.Filter.ElemMatch(x => x["UnCommonFields"], 
        //         Builders<Uncommonfield>.Filter.And(
        //             Builders<Uncommonfield>.Filter.Eq(x => x["k"], "ForeignAmount"),
        //             Builders<Uncommonfield>.Filter.In(f => f["v"], new object[]{ -10.78, -15.85 })
        //         )
        //     )
        // );
        //
        //
        // var filter3 = filterDefinitionBuilder.All(data => data.Aircraft, new []{"LOT", "Rayanir"});
        // var filter2 = filterDefinitionBuilder.ElemMatch(data => data.Aircraft, "LOT");
        // var filter5 = filterDefinitionBuilder.ElemMatch(data => data.Aircraft, Builders<char>.Filter.Eq(x => x, 'k'));
        //
        // // var filter6 = filterDefinitionBuilder.And(filter5, filter2);
        // var filter6 = filterDefinitionBuilder.And(Builders<FlightData>.Filter.Eq(p => p.Aircraft, "Rayanir"), Builders<FlightData>.Filter.Eq(p => p.DepartureAirport, "KRK"));
        //
        var results = await collection.FindAsync(filter);
        //var results2 = await collection.FindAsync(filter2);
        
        var singleResult = await results.FirstOrDefaultAsync();
        //var singleResult2 = await results2.FirstOrDefaultAsync();
        
        if (results.Any())
        {
            Console.WriteLine("Document has been found");
        }
        else
        {
            Console.WriteLine("There is no such document in the collection");
        }
        
    }

}
