using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CustomServiceRegistry.RegistryApi.Collections
{
    public class CentralRegistryCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }


        [BsonRepresentation(BsonType.String)]
        public Guid ServiceId { get; set; }


        [BsonRepresentation(BsonType.String)]
        public Guid TenantId { get; set; }

        public string ServiceName { get; set; }
        public string Scheme { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string HealthCheckUrl { get; set; }
    }
}
