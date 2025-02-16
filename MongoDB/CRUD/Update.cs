using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Model;

namespace MongoDB.CRUD;

public class Update
{
    private const string FlightDataCollectionName = "flightData";
    private const string ConnectionString = "mongodb://localhost:27017";
    private const string Flights = "flights";
    private readonly MongoClient _server;

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
                new[] { new Hobby { Title = "Coffee", Frequency = 7 } });
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

    public async Task LoadDataForUser()
    {
        var user = new User
        {
            Name = "Maria",
            Age = 29,
            Hobbies = new List<Hobby>
            {
                new() { Title = "Hiking", Frequency = 2 },
                new() { Title = "Good wine", Frequency = 3 },
                new() { Title = "Good wine", Frequency = 4 }
            }
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