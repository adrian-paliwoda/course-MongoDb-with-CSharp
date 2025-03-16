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

    public async Task Example_0()
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

        var pipeline = PipelineDefinition<Person, BsonDocument>
            .Create(new IPipelineStageDefinition[] { matchStage, groupStage });

        var results = await collection
            .Aggregate(pipeline)
            .ToListAsync();



        Console.WriteLine(results.Count);
    }
}