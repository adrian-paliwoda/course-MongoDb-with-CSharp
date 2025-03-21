using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Model;
using MongoDB.Utils;
using IPipelineStageDefinition = MongoDB.Driver.IPipelineStageDefinition;
using MongoClient = MongoDB.Driver.MongoClient;
using PipelineStageDefinitionBuilder = MongoDB.Driver.PipelineStageDefinitionBuilder;

namespace MongoDB.CRUD;

public class ReadWithAggregations
{
    private readonly MongoClient _server;

    public ReadWithAggregations()
    {
        _server = new MongoClient(DatabaseNames.ConnectionString);
    }

    public async Task Example_0_Group()
    {
        var database = _server.GetDatabase(DatabaseNames.PersonsDbnName);


        var collection = database.GetCollection<Person>(DatabaseNames.PersonsCollectionName);
        var matchStage = PipelineStageDefinitionBuilder.Match(Builders<Person>.Filter.Eq(p => p.Gender, "female")
        );

        var groupStage = PipelineStageDefinitionBuilder.Group<Person>(
            new BsonDocument
            {
                { "_id", new BsonDocument("state", "$location.state") },
                { "totalPersons", new BsonDocument("$sum", 1) }
            }
        );

        var sortBuilder = Builders<BsonDocument>.Sort;
        var sortStage = sortBuilder.Ascending("totalPersons");

        var pipeline = PipelineDefinition<Person, BsonDocument>
            .Create(new IPipelineStageDefinition[]
                { matchStage, groupStage, PipelineStageDefinitionBuilder.Sort(sortStage) });

        var resultsAsync = await collection
            .AggregateAsync(pipeline);

        var results = await resultsAsync.ToListAsync();

        Console.WriteLine(results);
        MongoDbHelper.ShowDocuments(results);
    }

    public async Task Example_1_Sort()
    {
        var database = _server.GetDatabase(DatabaseNames.PersonsDbnName);


        var collection = database.GetCollection<Person>(DatabaseNames.PersonsCollectionName);

        var resultsAggregateFluent = collection.Aggregate()
            .Match(Builders<Person>.Filter.Eq(p => p.Gender, "female"))
            .Group<BsonDocument>(new BsonDocument
            {
                { "_id", new BsonDocument("state", "$location.state") },
                { "totalPersons", new BsonDocument("$sum", 1) }
            })
            .Sort(new BsonDocumentSortDefinition<BsonDocument>(new BsonDocument("totalPersons", -1)));

        var results = await resultsAggregateFluent.ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_3_Projection()
    {
        var database = _server.GetDatabase(DatabaseNames.PersonsDbnName);
        var collection = database.GetCollection<Person>(DatabaseNames.PersonsCollectionName);

        var results = await collection.Aggregate()
            .Skip(100)
            .Limit(20).Project(person => new
            {
                person.Name,
                person.Email,
                person.Gender,
                Location = new
                {
                    type = "Point", person.Location.Coordinates
                }
            }).ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_4_Projection()
    {
        var database = _server.GetDatabase(DatabaseNames.PersonsDbnName);
        var collection = database.GetCollection<Person>(DatabaseNames.PersonsCollectionName);

        var results = await collection.Aggregate()
            .Skip(100)
            .Limit(20).Project(person => new
            {
                person.Name,
                person.Email,
                person.Gender,
                Location = new
                {
                    type = "Point", person.Location.Coordinates
                }
            }).Project(p => new
            {
                FullName = p.Name.First.ToUpper().Substring(0, 1) + p.Name.First.Substring(1) + " " +
                           p.Name.Last.ToUpper().Substring(1) + p.Name.Last.Substring(1),
                p.Gender,
                p.Location
            }).ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_5_Projection_Convert()
    {
        var database = _server.GetDatabase(DatabaseNames.PersonsDbnName);
        var collection = database.GetCollection<Person>(DatabaseNames.PersonsCollectionName);

        var pipeline = new BsonDocument[]
        {
            new("$project", new BsonDocument
            {
                { "Name", 1 },
                { "Email", 1 },
                { "Gender", 1 },
                {
                    "Location", new BsonDocument
                    {
                        { "type", "Point" },
                        {
                            "coordinates", new BsonArray
                            {
                                new BsonDocument("$convert", new BsonDocument
                                {
                                    { "input", "$location.coordinates.latitude" },
                                    { "to", "double" },
                                    { "onError", 0 }
                                }),
                                new BsonDocument("$convert", new BsonDocument
                                {
                                    { "input", "$location.coordinates.longitude" },
                                    { "to", "double" },
                                    { "onError", 0 }
                                })
                            }
                        }
                    }
                }
            })
        };


        var results = await (await collection
            .AggregateAsync<BsonDocument>(pipeline)).ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_6_Projection_ConvertDate()
    {
        var database = _server.GetDatabase(DatabaseNames.PersonsDbnName);
        var collection = database.GetCollection<Person>(DatabaseNames.PersonsCollectionName);

        var pipeline = new BsonDocument[]
        {
            new("$project", new BsonDocument
            {
                { "Name", 1 },
                { "Email", 1 },
                { "Gender", 1 },
                {
                    "Birthdate", new BsonDocument("$convert", new BsonDocument
                    {
                        { "input", "$dob.date" },
                        { "to", "date" },
                        { "onError", new BsonDateTime(DateTime.Now) },
                        { "onNull", new BsonDateTime(DateTime.Now) }
                    })
                }
            })
        };


        var results = await (await collection
            .AggregateAsync<BsonDocument>(pipeline)).ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_7_Group_ByDate()
    {
        var database = _server.GetDatabase(DatabaseNames.PersonsDbnName);
        var collection = database.GetCollection<Person>(DatabaseNames.PersonsCollectionName);

        var results = await collection
            .Aggregate()
            .Project(new BsonDocument("birthday", new BsonDocument("$toDate", "$dob.date")))
            .Group(new BsonDocument
            {
                { "_id", new BsonDocument("birthYear", new BsonDocument("$isoWeekYear", "$birthday")) },
                { "numPerson", new BsonDocument("$sum", 1) }
            })
            .ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_8_Group_Unwind_And_Push()
    {
        var database = _server.GetDatabase(DatabaseNames.UserDatabaseName);
        var collection = database.GetCollection<User>(DatabaseNames.UserCollectionName);

        var results = await collection
            .Aggregate()
            .Unwind(p => p.Hobbies)
            .Group(new BsonDocument
                {
                    { "_id", new BsonDocument("age", "$age") },
                    { "allHobbies", new BsonDocument("$push", "$hobbies") }
                }
            )
            .ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_9_Group_Unwind_And_AddToSet()
    {
        var database = _server.GetDatabase(DatabaseNames.UserDatabaseName);
        var collection = database.GetCollection<User>(DatabaseNames.UserCollectionName);

        var results = await collection
            .Aggregate()
            .Unwind(p => p.Hobbies)
            .Group(new BsonDocument
                {
                    { "_id", new BsonDocument("age", "$age") },
                    { "allHobbies", new BsonDocument("$addToSet", "$hobbies") }
                }
            )
            .ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_10_Project_Slice()
    {
        var database = _server.GetDatabase(DatabaseNames.FriendsDatabaseName);
        var collection = database.GetCollection<Friend>(DatabaseNames.FriendsCollectionName);

        var results = await collection
            .Aggregate()
            .Project(p => new
            {
                p.Name,
                p.Hobbies,
                p.Age,
                p.ExamScores[2].Score
            })
            .ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_11_Project_Size()
    {
        var database = _server.GetDatabase(DatabaseNames.FriendsDatabaseName);
        var collection = database.GetCollection<Friend>(DatabaseNames.FriendsCollectionName);

        var results = await collection
            .Aggregate()
            .Project(p => new
            {
                p.Name,
                p.Hobbies,
                p.Age,
                Score = p.ExamScores.Count
            })
            .ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_12_Project_FilterInProjection()
    {
        var database = _server.GetDatabase(DatabaseNames.FriendsDatabaseName);
        var collection = database.GetCollection<Friend>(DatabaseNames.FriendsCollectionName);

        var results = await collection
            .Aggregate()
            .Project(p => new
            {
                p.Name,
                p.Hobbies,
                p.Age,
                Scores = p.ExamScores.Where(p => p.Score > 60)
            })
            .ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_13_Group()
    {
        var database = _server.GetDatabase(DatabaseNames.FriendsDatabaseName);
        var collection = database.GetCollection<Friend>(DatabaseNames.FriendsCollectionName);

        var results = await collection
            .Aggregate()
            .Unwind(p => p.ExamScores)
            .Project(new BsonDocument
            {
                { "name", 1 },
                { "age", 1 },
                { "score", "$examScores.score" }
            })
            .Sort(new BsonDocumentSortDefinition<BsonDocument>(new BsonDocument("score", -1)))
            .Group(new BsonDocument
            {
                { "_id", "$_id" },
                { "name", new BsonDocument("$first", "$name") },
                { "maxScore", new BsonDocument("$max", "$score") }
            })
            .Sort(new BsonDocument("maxScore", -1))
            .ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_14_Bucket()
    {
        var database = _server.GetDatabase(DatabaseNames.PersonsDbnName);
        var collection = database.GetCollection<Person>(DatabaseNames.PersonsCollectionName);

        var results = await collection
            .Aggregate()
            .Bucket(
                p => p.Dob.Age.HasValue ? p.Dob.Age.Value : 0,
                new[] { 18, 30, 40, 50, 60, 120 },
                output => new
                {
                    output.Key,
                    numPersons = output.Count(),
                    averageAge = output.Average(p => p.Dob.Age)
                }, new AggregateBucketOptions<int>
                {
                    DefaultBucket = new Optional<int>(1)
                }
            )
            .ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_15_BucketAuto()
    {
        var database = _server.GetDatabase(DatabaseNames.PersonsDbnName);
        var collection = database.GetCollection<Person>(DatabaseNames.PersonsCollectionName);

        var results = await collection
            .Aggregate()
            .BucketAuto(p => p.Dob.Age,
                5,
                o => new
                {
                    numPersons = o.Count(),
                    averageAge = o.Average(p => p.Dob.Age)
                })
            .ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_16_Geo()
    {
        var database = _server.GetDatabase(DatabaseNames.PersonsDbnName);
        var collection = database.GetCollection<BsonDocument>(DatabaseNames.PersonsCollectionName);

        var pipeline = new[]
        {
            new BsonDocument("$geoNear", new BsonDocument
            {
                {
                    "near", new BsonDocument
                    {
                        { "type", "Point" },
                        { "coordinates", new BsonArray { -18.4, -42.8 } }
                    }
                },
                { "maxDistance", 1000000 },
                { "query", new BsonDocument("age", new BsonDocument("$gt", 30)) },
                { "distanceField", "distance" }
            }),
            new BsonDocument("$limit", 10)
        };

        var results = await collection
            .Aggregate<BsonDocument>(pipeline)
            .ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_17_Geo_Near()
    {
        var database = _server.GetDatabase(DatabaseNames.PersonsDbnName);
        var collection = database.GetCollection<BsonDocument>(DatabaseNames.PersonsCollectionName);

        var filter = Builders<BsonDocument>.Filter.Near(p => p["distance"], maxDistance: 1000000, minDistance: 1,
            point: new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(-18.4, -42.8)));

        var results = await collection
            .Find(filter).ToListAsync();

        Console.WriteLine(results.Count);
    }
}