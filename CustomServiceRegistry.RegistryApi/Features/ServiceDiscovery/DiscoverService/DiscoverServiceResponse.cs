using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.DiscoverService;

public class DiscoverServiceResponse
{
    public Guid ServiceId { get; set; }
    public Guid TenantId { get; set; }
    public string ServiceName { get; set; }
    public string Scheme { get; set; }
    public string HostName { get; set; }
    public int Port { get; set; }
    public string HealthCheckUrl { get; set; }
}
