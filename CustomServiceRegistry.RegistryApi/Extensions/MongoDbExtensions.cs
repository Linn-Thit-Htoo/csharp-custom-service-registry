using MongoDB.Driver;

namespace CustomServiceRegistry.RegistryApi.Extensions
{
    public static class MongoDbExtensions
    {
        public static IMongoCollection<T> GetCollection<T>(this string collectionName)
        {
            var client = new MongoClient(
                "mongodb+srv://mglinnthithtoo:IVIxNP41oHNh6rq3@cluster1.gwvyz.mongodb.net/"
            );
            var database = client.GetDatabase("ServiceRegistry");
            var collection = database.GetCollection<T>(collectionName);

            return collection;
        }
    }
}
