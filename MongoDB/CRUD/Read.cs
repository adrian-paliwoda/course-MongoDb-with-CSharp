using MongoDB.Bson;
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

        var genresToLookFor = new[] { "Horror", "Drama" };
        var filter = Builders<Movie>.Filter.All(m => m.Genres, genresToLookFor);
        var filter2 = Builders<Movie>.Filter.And(Builders<Movie>.Filter.All(m => m.Genres, genresToLookFor),
            Builders<Movie>.Filter.Size(m => m.Genres, genresToLookFor.Length));
        var filter3 = Builders<Movie>.Filter.AnyEq(m => m.Genres, "Drama");
        var filter4 = Builders<Movie>.Filter.AnyIn(m => m.Genres, genresToLookFor);

        var results = await collection.FindAsync(filter);
        var results2 = await collection.FindAsync(filter2);
        var results3 = await collection.FindAsync(filter3);
        var results4 = await collection.FindAsync(filter4);

        var movies = await results.ToListAsync();
        var movies2 = await results2.ToListAsync();
        var movies3 = await results3.ToListAsync();
        var movies4 = await results4.ToListAsync();

        var count1 = movies.Count;
        var count2 = movies2.Count;
        var count3 = movies3.Count;
        var count4 = movies4.Count;

        Console.WriteLine($"There are {count1} documents in the collection");
        Console.WriteLine($"There are {count2} documents in the collection");
        Console.WriteLine($"There are {count3} documents in the collection");
        Console.WriteLine($"There are {count4} documents in the collection");

        var singleResult = movies.FirstOrDefault();
        var singleResultMovie = movies.FirstOrDefault();

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

        if (items.Count != 0)
        {
            Console.WriteLine("Document has been found");
            Console.WriteLine(items.FirstOrDefault()?.Name);
        }
        else
        {
            Console.WriteLine("There is no such document in the collection");
        }
    }

    public async Task Read_Example_4_Without_Types(string databaseName, string collectionName)
    {
        var database = _server.GetDatabase(databaseName);
        var collection = database.GetCollection<BsonDocument>(collectionName);
        
        var filter = Builders<BsonDocument>.Filter.ElemMatch(
            "hobbies",
            Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("freq", 3),
                Builders<BsonDocument>.Filter.Eq("title", "Sports")
            )
        );
        var results = await collection.FindAsync(filter);

        var items = await results.ToListAsync();

        if (items.Count != 0)
        {
            Console.WriteLine("Document has been found");
            Console.WriteLine(items.FirstOrDefault()?["name"]);
        }
        else
        {
            Console.WriteLine("There is no such document in the collection");
        }
    }
}