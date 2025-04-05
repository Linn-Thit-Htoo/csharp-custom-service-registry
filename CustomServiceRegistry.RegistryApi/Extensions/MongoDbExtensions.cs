using MongoDB.Driver;

namespace CustomServiceRegistry.RegistryApi.Extensions;

public static class MongoDbExtensions
{
    public static IMongoCollection<T> GetCollection<T>(this string collectionName)
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("ServiceRegistry");
        var collection = database.GetCollection<T>(collectionName);

        return collection;
    }
}
