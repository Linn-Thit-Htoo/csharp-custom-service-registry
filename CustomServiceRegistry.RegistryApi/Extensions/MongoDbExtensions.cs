global using CustomServiceRegistry.RegistryApi.Constants;
global using MongoDB.Driver;

namespace CustomServiceRegistry.RegistryApi.Extensions;

public static class MongoDbExtensions
{
    public static IMongoCollection<T> GetCollection<T>(this string collectionName)
    {
        string environmentName = Environment.GetEnvironmentVariable(
            ApplicationConstants.Environment
        )!;
        string connectionString = string.Empty;

        connectionString =
            !environmentName.IsNullOrWhiteSpace()
            && environmentName.Equals(ApplicationConstants.DevelopmentEnvironment)
                ? ApplicationConstants.LocalMongoConnectionString
                : ApplicationConstants.ContainerizedMongoConnectionString;

        if (connectionString.IsNullOrWhiteSpace())
            throw new ArgumentNullException("Connection String cannot be empty.");

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(ApplicationConstants.DatabaseName);
        var collection = database.GetCollection<T>(collectionName);

        return collection;
    }
}
