using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Model;
using MongoDB.Utils;

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
        var matchStage = PipelineStageDefinitionBuilder.Match(
            Builders<Person>.Filter.Eq(p => p.Gender, "female")
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

        var resultsAggregateFluent = collection
            .Aggregate()
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

        var results = await collection
            .Aggregate()
            .Skip(100)
            .Limit(20)
            .Project(person => new
            {
                person.Name,
                person.Email,
                person.Gender,
                Location = new
                {
                    type = "Point", person.Location.Coordinates
                }
            })
            .ToListAsync();

        Console.WriteLine(results.Count);
    }

    public async Task Example_4_Projection()
    {
        var database = _server.GetDatabase(DatabaseNames.PersonsDbnName);
        var collection = database.GetCollection<Person>(DatabaseNames.PersonsCollectionName);

        var results = await collection
            .Aggregate()
            .Skip(100)
            .Limit(20)
            .Project(person => new
            {
                person.Name,
                person.Email,
                person.Gender,
                Location = new
                {
                    type = "Point", person.Location.Coordinates
                }
            })
            .Project(p => new
            {
                FullName = p.Name.First.ToUpper().Substring(0, 1) + p.Name.First.Substring(1) + " " +
                           p.Name.Last.ToUpper().Substring(1) + p.Name.Last.Substring(1),
                p.Gender,
                p.Location
            })
            .ToListAsync();

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
                .AggregateAsync<BsonDocument>(pipeline))
            .ToListAsync();

        Console.WriteLine(results.Count);
    }
}