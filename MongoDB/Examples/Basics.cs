using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Model;
using MongoDB.Utils;

namespace MongoDB.Examples;

public static class Basics
{
    private const string MongodbLocalhost = "mongodb://localhost:27017";
    private const string DatabaseShopName = "shop";
    private const string FlightsDbName = "flights";
    private const string PassengersCollectionName = "passengers";
    private const string ClinicDbName = "clinic";
    private const string PatientsCollectionName = "patients";

    public static void Example_0()
    {
        Console.WriteLine("Hello, MongoDB!");

        var mongoClient = new MongoClient(new MongoUrl(MongodbLocalhost));
        var databaseNames = mongoClient.ListDatabaseNames().ToList();

        for (var i = 0; i < databaseNames.Count; i++)
        {
            Console.WriteLine(databaseNames[i]);
        }
        
        var database = mongoClient.GetDatabase(DatabaseShopName);
        var collection = database.GetCollection<Product>("products");
        var documents = collection.FindAsync(Builders<Product>.Filter.Empty).Result.ToList();


        var flightsDb = mongoClient.GetDatabase(FlightsDbName);
        var flightData = flightsDb.GetCollection<FlightData>("flightData");

        var document = new FlightData("AU", "KRK", "Rayanir", 1222, true);

        var filterDefinitionBuilder = new FilterDefinitionBuilder<FlightData>();
        var filter = filterDefinitionBuilder.Eq(p => p.DepartureAirport, document.DepartureAirport)
                     & filterDefinitionBuilder.Eq(p => p.ArrivalAirport, document.ArrivalAirport)
                     & filterDefinitionBuilder.Eq(p => p.Aircraft, document.Aircraft)
                     & filterDefinitionBuilder.Eq(p => p.Distance, document.Distance)
                     & filterDefinitionBuilder.Eq(p => p.Intercontinental, document.Intercontinental);

        var exists = flightData.FindAsync(filter).Result.Any(); // By value
        if (!exists)
        {
            flightData.InsertOne(document);
        }

        var data = flightData.FindAsync(FilterDefinition<FlightData>.Empty).Result.ToList();

        for (var i = 0; i < data.Count; i++)
        {
            Console.WriteLine(data[i].Id);
        }

        Console.WriteLine("End");
    }

    public static void Example_2()
    {
        var mongoClient = new MongoClient(new MongoUrl(MongodbLocalhost));
        var db = mongoClient.GetDatabase("flights");

        var passengerProjection =
            Builders<Passenger>.Projection.Include(p => p.Name).Include(p => p.Age).Exclude(p => p.Id);
        
        var collection = db.GetCollection<Passenger>(PassengersCollectionName);
        var collectionDocuments = collection.Find(FilterDefinition<Passenger>.Empty).Project(passengerProjection).ToList();
                           
        for (var i = 0; i < collectionDocuments.Count; i++)
        {
            Console.WriteLine(collectionDocuments[i].ToString());
        }
    }

    public static void Example_3()
    {
        var client = new MongoClient(new MongoUrl(MongodbLocalhost));
        var database = client.GetDatabase(ClinicDbName);
        var patients = database.GetCollection<Patient>(PatientsCollectionName);

        var patientsItems = new[]
        {
            new Patient("Adrian", "Pawel", 29,
                new[]
                {
                    new History("cold", "take medicines"),
                    new History("OCD", "therapy")
                }),
            new Patient("Zuza", "Karolina", 27,
                new[]
                {
                    new History("allergy", "take medicines"),
                    new History("cold", "stay in bed")
                }),
            new Patient("Max", "Schwarzmueller", 29,
                new[]
                {
                    new History("cold", "take medicines"),
                    new History("allergy", "take drugs")
                }),
        };

        var updateDefinitionBuilder = new UpdateDefinitionBuilder<Patient>();
        var update = updateDefinitionBuilder.Push(p => p.History, new History("overdose coffee", "more coffee"))
                                            .Set(i => i.Age, 30 );
        
        patients.InsertMany(patientsItems);
        var foundPatients = patients.FindAsync(p => p.Age > 28).Result.ToList();
        patients.DeleteMany(p => p.Age < 29);
        patients.UpdateMany(p => p.Age > 20 && p.History.Any(i => i.Disease.Equals("cold")), update);
        patients.DeleteMany(p => p.History.Any(i => i.Disease.Equals("cold")));

        for (int i = 0; i < foundPatients.Count; i++)
        {
            Console.WriteLine(foundPatients[i].FirstName + " " + foundPatients[i].LastName);
        }

    }

    public static async Task Example_4_Import()
    {
        const string databaseName = "movieData";
        var collectionName = "movies";
        var fileToCollectionJson = "./Assets/tv-shows.json";
        
        var client = new MongoClient(new MongoUrl(MongodbLocalhost));
        var mongoDb = client.GetDatabase(databaseName);

        MongoDbHelper.ImportDatabaseFromFile(mongoDb, collectionName, fileToCollectionJson);

        var collection = mongoDb.GetCollection<BsonDocument>(collectionName);
        
        var document = await collection.Find(new BsonDocument()).Limit(2).ToListAsync();

        for (int i = 0; i < document.Count; i++)
        {
            Console.WriteLine(document[i].ToString());
        }

    }
}