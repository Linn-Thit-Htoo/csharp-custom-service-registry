using MongoDB.Bson;

namespace CustomServiceRegistry.RegistryApi.Collections;

public class TenantRateLimiterCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid RateLimiterId { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid TenantId { get; set; }
    public int TotalRequest { get; set; }
    public DateTime CreatedAt { get; set; }
}
