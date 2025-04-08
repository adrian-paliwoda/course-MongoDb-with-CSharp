using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Model;
using MongoDB.Utils;

namespace MongoDB.CRUD;

public class Update
{
    private readonly MongoClient _server;

    public Update()
    {
        _server = new MongoClient(DatabaseNames.ConnectionString);
    }

    public void Example_One()
    {
        var database = _server.GetDatabase(DatabaseNames.FlightsDbName);
        if (database != null)
        {
            var collection = database.GetCollection<FlightData>(DatabaseNames.FlightDataCollectionName);
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
        var database = _server.GetDatabase(DatabaseNames.FlightsDbName);
        if (database != null)
        {
            var collection = database.GetCollection<FlightData>(DatabaseNames.FlightDataCollectionName);
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
        var database = _server.GetDatabase(DatabaseNames.FlightsDbName);
        if (database != null)
        {
            var collection = database.GetCollection<FlightData>(DatabaseNames.FlightDataCollectionName);
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

    public async Task Example_2()
    {
        var database = _server.GetDatabase("usersData");
        if (database != null)
        {
            var collection = database.GetCollection<User>("users");
            var filter = Builders<User>.Filter.Eq(m => m.Id, new ObjectId("67ab8e505693c72d9161cb1f"));

            var result = await collection.FindAsync(filter);
            var document = await result.FirstOrDefaultAsync();

            var update = Builders<User>.Update.PopFirst(m => m.Hobbies);
            var resultUpdate = await collection.UpdateManyAsync(filter, update, new UpdateOptions { IsUpsert = true });

            Console.WriteLine(document.Name);
        }
    }

    public async Task Example_3()
    {
        await LoadDataForUser();

        var database = _server.GetDatabase("usersData");
        if (database != null)
        {
            var collection = database.GetCollection<User>("users");
            var filter = Builders<User>.Filter.Eq(m => m.Id, new ObjectId("67ab8e505693c72d9161cb1f"));

            var result = await collection.FindAsync(filter);
            var document = await result.FirstOrDefaultAsync();

            var update = Builders<User>.Update.Push(u => u.Hobbies, new Hobby { Title = "Coffee", Frequency = 7 });
            var resultUpdate = await collection.UpdateOneAsync(filter, update);

            Console.WriteLine(document.Name);
        }
    }

    public async Task Example_4()
    {
        await LoadDataForUser();

        var database = _server.GetDatabase("usersData");
        if (database != null)
        {
            var collection = database.GetCollection<User>("users");
            var filter = Builders<User>.Filter.Eq(m => m.Id, new ObjectId("67ab8e505693c72d9161cb1f"));

            var result = await collection.FindAsync(filter);
            var document = await result.FirstOrDefaultAsync();

            var update = Builders<User>.Update.PushEach(u => u.Hobbies,
                [new Hobby { Title = "Coffee", Frequency = 7 }]);
            var resultUpdate = await collection.UpdateManyAsync(FilterDefinition<User>.Empty, update);

            Console.WriteLine(document.Name);
        }
    }

    public async Task Example_5()
    {
        await LoadDataForUser();

        var database = _server.GetDatabase("usersData");
        if (database != null)
        {
            var collection = database.GetCollection<User>("users");
            var filter = Builders<User>.Filter.Eq(m => m.Id, new ObjectId("67ab8e505693c72d9161cb1f"));

            var result = await collection.FindAsync(filter);
            var document = await result.FirstOrDefaultAsync();

            // Need to be exacly the same with new Hobby
            // var update = Builders<User>.Update.Pull(m => m.Hobbies, new Hobby { Title = "Good wine", Frequency = 1});

            var update =
                Builders<User>.Update.PullFilter(m => m.Hobbies, Builders<Hobby>.Filter.Eq(h => h.Title, "Good wine"));
            var resultUpdate = await collection.UpdateManyAsync(FilterDefinition<User>.Empty, update);

            Console.WriteLine(document.Name);
        }
    }

    public async Task Example_6()
    {
        await LoadDataForUser();

        var database = _server.GetDatabase("usersData");
        if (database != null)
        {
            var collection = database.GetCollection<User>("users");
            var filter = Builders<User>.Filter.ElemMatch(m => m.Hobbies, h => h.Frequency >= 2);

            var result = await collection.FindAsync(filter);
            var document = await result.FirstOrDefaultAsync();

            // Need to be exacly the same with new Hobby
            // var update = Builders<User>.Update.Pull(m => m.Hobbies, new Hobby { Title = "Good wine", Frequency = 1});

            var update = Builders<User>.Update.Set("hobbies.$[el].goodFrequencyTest", true);

            var arrayFilter =
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("el.frequency",
                    new BsonDocument("$gte", 2)));
            var arrayFilters = new List<ArrayFilterDefinition>();
            arrayFilters.Add(arrayFilter);

            var resultUpdate =
                await collection.UpdateManyAsync(filter, update, new UpdateOptions { ArrayFilters = arrayFilters });

            Console.WriteLine(document.Name);
        }
    }

    public static void Example_7()
    {
        var client = new MongoClient(new MongoUrl(DatabaseNames.MongodbLocalhost));
        var database = client.GetDatabase(DatabaseNames.ClinicDbName);
        var patients = database.GetCollection<Patient>(DatabaseNames.PatientsCollectionName);

        var patientsItems = new[]
        {
            new Patient("Adrian", "Pawel", 29,
            [
                new History("cold", "take medicines"),
                    new History("OCD", "therapy")
            ]),
            new Patient("Zuza", "Karolina", 27,
            [
                new History("allergy", "take medicines"),
                    new History("cold", "stay in bed")
            ]),
            new Patient("Max", "Schwarzmueller", 29,
            [
                new History("cold", "take medicines"),
                    new History("allergy", "take drugs")
            ])
        };

        var updateDefinitionBuilder = new UpdateDefinitionBuilder<Patient>();
        var update = updateDefinitionBuilder.Push(p => p.History, new History("overdose coffee", "more coffee"))
            .Set(i => i.Age, 30);

        patients.InsertMany(patientsItems);
        var foundPatients = patients.FindAsync(p => p.Age > 28).Result.ToList();
        patients.DeleteMany(p => p.Age < 29);
        patients.UpdateMany(p => p.Age > 20 && p.History.Any(i => i.Disease.Equals("cold")), update);
        patients.DeleteMany(p => p.History.Any(i => i.Disease.Equals("cold")));

        for (var i = 0; i < foundPatients.Count; i++)
        {
            Console.WriteLine(foundPatients[i].FirstName + " " + foundPatients[i].LastName);
        }
    }

    public async Task LoadDataForUser()
    {
        var user = new User
        {
            Name = "Maria",
            Age = 29,
            Hobbies =
            [
                new() { Title = "Hiking", Frequency = 2 },
                new() { Title = "Good wine", Frequency = 3 },
                new() { Title = "Good wine", Frequency = 4 }
            ]
        };

        var database = _server.GetDatabase("usersData");
        if (database != null)
        {
            var collection = database.GetCollection<User>("users");

            var filter = Builders<User>.Filter.Eq(u => u.Name, user.Name);
            var existingUser = await collection.Find(filter).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                user.Id = existingUser.Id;
                var result = await collection.ReplaceOneAsync(u => u.Name == user.Name, user,
                    new ReplaceOptions { IsUpsert = true });
            }
            else
            {
                await collection.InsertOneAsync(user);
            }
        }
    }
}