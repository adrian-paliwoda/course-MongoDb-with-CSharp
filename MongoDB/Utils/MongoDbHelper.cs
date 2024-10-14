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
}