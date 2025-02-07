using MongoDB.Driver;
using MongoDB.Model;

namespace MongoDB.CRUD;

public class Read
{
    private const string FlightDataCollectionName = "flightData";
    private const string ConnectionString = "mongodb://localhost:27017";
    private const string Flights = "flights";
    private readonly IMongoDatabase? _database;
    private readonly MongoClient _server;

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
            for (var i = 0; i < documents.Count; i++)
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

    public async Task Read_Example_2(string databaseName, string collectionName)
    {
        var database = _server.GetDatabase(databaseName);
        var collection = database.GetCollection<Movie>(collectionName);

        var filter = Builders<Movie>.Filter.And(
            Builders<Movie>.Filter.Gt(m => m.Rating.Average, 8),
            Builders<Movie>.Filter.Lte(m => m.Rating.Average, 10)
        );
        var results = await collection.FindAsync(filter);

        var singleResult = await results.FirstOrDefaultAsync();

        if (singleResult != null)
        {
            Console.WriteLine("Document has been found");
        }
        else
        {
            Console.WriteLine("There is no such document in the collection");
        }
    }

    public async Task Read_Example_3(string databaseName, string collectionName)
    {
        var database = _server.GetDatabase(databaseName);
        var collection = database.GetCollection<Movie>(collectionName);

        var filter = Builders<Movie>.Filter.All(m => m.Genres, new[] { "Horror", "Drama" });
        var results = await collection.FindAsync(filter);

        var singleResult = await results.FirstOrDefaultAsync();

        if (singleResult != null)
        {
            Console.WriteLine("Document has been found");
        }
        else
        {
            Console.WriteLine("There is no such document in the collection");
        }
    }
    
    public async Task Read_Example_4(string databaseName, string collectionName)
    {
        var database = _server.GetDatabase(databaseName);
        var collection = database.GetCollection<User>(collectionName);

        // var filter = Builders<User>.Filter.ElemMatch(u => u.Hobbies, h => h.Frequency == 3 && h.Title == "Sports");
        var filter = Builders<User>.Filter.ElemMatch(u => u.Hobbies, Builders<Hobby>.Filter.And(
            Builders<Hobby>.Filter.Eq(h => h.Frequency, 3),
            Builders<Hobby>.Filter.Eq(h => h.Title, "Sports")));
        var results = await collection.FindAsync(filter);

        var items = await results.ToListAsync(); 

        if (items.Any())
        {
            Console.WriteLine("Document has been found");
        }
        else
        {
            Console.WriteLine("There is no such document in the collection");
        }
    }
    
}