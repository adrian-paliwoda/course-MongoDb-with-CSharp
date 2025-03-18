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

    public async Task Example_1()
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

    public async Task Example_3()
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

        var resultsAsync = await collection
            .AggregateAsync(pipeline);
        var results = await resultsAsync.ToListAsync();

        Console.WriteLine(results);
        MongoDbHelper.ShowDocuments(results);
    }
}