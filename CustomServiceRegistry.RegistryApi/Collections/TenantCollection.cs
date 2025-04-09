namespace CustomServiceRegistry.RegistryApi.Collections;

public class TenantCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid TenantId { get; set; }
    public string ApplicationName { get; set; }
}
