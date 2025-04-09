
namespace CustomServiceRegistry.RegistryApi.Collections;

public class ServiceLogCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid LogId { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid TenantId { get; set; }

    public DateTime LogAt { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public CentralRegistryCollection ServiceInfo { get; set; }
}
