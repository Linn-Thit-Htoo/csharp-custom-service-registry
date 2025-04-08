using CustomServiceRegistry.RegistryApi.Constants;
using MongoDB.Driver;

namespace CustomServiceRegistry.RegistryApi.Extensions;

public static class MongoDbExtensions
{
    public static IMongoCollection<T> GetCollection<T>(this string collectionName)
    {
        string environmentName = Environment.GetEnvironmentVariable(
            ApplicationConstants.Environment
        )!;
        string connectionString = string.Empty;

        if (
            !environmentName.IsNullOrWhiteSpace()
            && environmentName.Equals(ApplicationConstants.DevelopmentEnvironment)
        )
        {
            connectionString = ApplicationConstants.LocalMongoConnectionString;
        }
        else
        {
            connectionString = ApplicationConstants.ContainerizedMongoConnectionString;
        }

        if (connectionString.IsNullOrWhiteSpace())
            throw new ArgumentNullException("ConnectionString cannot be empty.");

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(ApplicationConstants.DatabaseName);
        var collection = database.GetCollection<T>(collectionName);

        return collection;
    }
}
