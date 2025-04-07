using MongoDB.Driver;

namespace CustomServiceRegistry.RegistryApi.Extensions;

public static class MongoDbExtensions
{
    public static IMongoCollection<T> GetCollection<T>(this string collectionName)
    {
        string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        string connectionString = string.Empty;

        if (!environmentName.IsNullOrWhiteSpace() && environmentName.Equals("Development"))
        {
            connectionString = "mongodb://localhost:27017";
        }
        else
        {
            connectionString = "mongodb://root:root@mongo:27017/?authSource=admin";
        }

        if (connectionString.IsNullOrWhiteSpace())
            throw new ArgumentNullException("ConnectionString cannot be empty.");

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("ServiceRegistry");
        var collection = database.GetCollection<T>(collectionName);

        return collection;
    }
}
