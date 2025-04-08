using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoDB.Utils;

public static class MongoDbHelper
{
    public static MongoClientSettings GenerateMongoDbSettings()
    {
        var certificate =
            X509CertificateLoader.LoadPkcs12FromFile(DatabaseNames.CertificateP12, new ReadOnlySpan<char>(DatabaseNames.CertificatePassword.ToCharArray()));
        var caCert = X509CertificateLoader.LoadCertificateFromFile(DatabaseNames.CaCertificate);

        return new MongoClientSettings
        {
            Server = new MongoServerAddress(DatabaseNames.Host, DatabaseNames.Port),
            Credential = MongoCredential.CreateCredential(DatabaseNames.AdminDatabaseName, DatabaseNames.UserName,
                DatabaseNames.Password),
            UseTls = true,
            ConnectTimeout = TimeSpan.FromSeconds(5),
            ServerSelectionTimeout = TimeSpan.FromSeconds(10),
            SslSettings = new SslSettings
            {
                ClientCertificates = [certificate],
                EnabledSslProtocols = SslProtocols.Tls12,
                ServerCertificateValidationCallback = (_, cert, chain, _) =>
                {
                    if (chain == null || cert is not X509Certificate2 certX509)
                    {
                        return false;
                    }

                    chain.ChainPolicy.ExtraStore.Add(caCert);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
                    return chain.Build(certX509);
                }
            }
        };
    }

    public static void ShowDocuments<T>(IMongoDatabase mongoDb, string collectionName)
    {
        ArgumentNullException.ThrowIfNull(mongoDb);

        var collection = mongoDb.GetCollection<T>(collectionName);
        var documents = collection.FindAsync(FilterDefinition<T>.Empty).Result.ToList();

        Console.WriteLine($"In mongoDb in collection {collectionName} there are documents:");
        for (var i = 0; i < documents.Count; i++)
        {
            Console.WriteLine(documents[i]?.ToString());
            Console.WriteLine();
        }
    }

    public static void ShowDocuments<T>(IReadOnlyCollection<T> documents)
    {
        Console.WriteLine($"In mongoDb in collection there are {documents.Count} documents:");
        foreach (var document in documents)
        {
            Console.WriteLine(document?.ToString());
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

    public static void ReadFromFile<T>(IMongoDatabase? database, string collectionName, string pathToJsonFile)
    {
        if (string.IsNullOrEmpty(pathToJsonFile) || string.IsNullOrWhiteSpace(pathToJsonFile))
        {
            pathToJsonFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ProjectNames.Assets,
                DatabaseNames.FlightDataExampleFileName);
        }

        if (string.IsNullOrEmpty(collectionName) || string.IsNullOrWhiteSpace(collectionName))
        {
            collectionName = DatabaseNames.FlightDataCollectionName;
        }

        if (database != null)
        {
            var collection = database.GetCollection<T>(collectionName);

            var bsonFromFile = File.ReadAllText(pathToJsonFile);
            var bsonDocuments = BsonSerializer.Deserialize<IList<T>>(bsonFromFile);

            collection.InsertMany(bsonDocuments);

            ShowDocuments<T>(database, collectionName);
        }
    }
}