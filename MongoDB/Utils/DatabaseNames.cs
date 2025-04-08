namespace MongoDB.Utils;

public static class DatabaseNames
{
    public const string ConnectionString = "mongodb://localhost:27017";
    public const string Host = "localhost";
    public const int Port = 27017;
    
    // Use Key Vault
    public const string UserName = "admin";
    public const string Password = "admin";
    
    // Use Key Vault
    public const string CertificateP12 = "/etc/mongoTls/mongodb.p12";
    public const string CaCertificate = "/etc/mongoTls/mongodb-cert.crt";
    public const string CertificatePassword = "qwert";
    
    public const string AdminDatabaseName = "admin";
    public const string FlightDataExampleFileName = "flightDataExample.json";
    public const string FlightDataCollectionName = "flightData";
    public const string PersonsDbnName = "personsData";
    public const string PersonsCollectionName = "persons";
    public const string MongodbLocalhost = "mongodb://localhost:27017";
    public const string DatabaseShopName = "shop";
    public const string FlightsDbName = "flights";
    public const string PassengersCollectionName = "passengers";
    public const string ClinicDbName = "clinic";
    public const string PatientsCollectionName = "patients";
    public const string UserDatabaseName = "usersData";
    public const string UserCollectionName = "users";
    public const string FriendsDatabaseName = "arrayData";
    public const string FriendsCollectionName = "arrays";
    
    public const string TotalPersons = "totalPersons";
    public const string Point = "Point";
    public const string Wizz = "WIZZ";
}