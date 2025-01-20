using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoDB.Utils;

public static class MongoDbHelper
{
    public static void ShowDocuments<T>(IMongoDatabase mongoDb, string collectionName)
    {
        if (mongoDb == null)
        {
            throw new ArgumentNullException(nameof(mongoDb));
        }
        
        var collection = mongoDb.GetCollection<T>(collectionName);
        var documents = collection.FindAsync(FilterDefinition<T>.Empty).Result.ToList();
        

        Console.WriteLine($"In mongoDb in collection {collectionName} there are documents:");
        for (int i = 0; i < documents.Count; i++)
        {
            Console.WriteLine(documents[i]?.ToString());
            Console.WriteLine();
        }
    }

    public static void ImportDatabaseFromFile(IMongoDatabase mongoDb, string collectionName, string pathToJsonFile)
    {
        var collection = mongoDb.GetCollection<BsonDocument>(collectionName);

        var jsonData = File.ReadAllText(pathToJsonFile);
        var bsonArray = BsonSerializer.Deserialize<BsonArray>(jsonData);

        for (var index = 0; index < bsonArray.Count; index++)
        {
            var bsonValue = bsonArray[(Index)index];
            var document = bsonValue.AsBsonDocument;
            
            collection.InsertOne(document);
        }
    }
}