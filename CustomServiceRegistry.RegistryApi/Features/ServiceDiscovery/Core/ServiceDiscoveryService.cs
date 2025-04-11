namespace CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.Core;

public class ServiceDiscoveryService : IServiceDiscoveryService
{
    private readonly IMongoCollection<CentralRegistryCollection> _centralRegistryCollection;

    public ServiceDiscoveryService()
    {
        _centralRegistryCollection =
            CollectionNames.CentralRegistryCollection.GetCollection<CentralRegistryCollection>();
    }

    public async Task<Result<DiscoverServiceResponse>> DiscoverServiceAsync(
        string serviceName,
        CancellationToken cs = default
    )
    {
        Result<DiscoverServiceResponse> result;
        var lst = new List<ServiceDiscoveryModel>();

        var item = await _centralRegistryCollection
            .Find(x => x.ServiceName.ToLower().Equals(serviceName.ToLower()))
            .FirstOrDefaultAsync(cancellationToken: cs);
        if (item is null)
        {
            result = Result<DiscoverServiceResponse>.NotFound("Service not found.");
            goto result;
        }

        lst.Add(new ServiceDiscoveryModel()
        {
            ServiceId = item.ServiceId,
            HealthCheckUrl = item.HealthCheckUrl,
            HostName = item.HostName,
            Port = item.Port,
            Scheme = item.Scheme,
            ServiceName = item.ServiceName,
            TenantId = item.TenantId
        });

        result = Result<DiscoverServiceResponse>.Success(new DiscoverServiceResponse()
        {
            Services = lst
        });

    result:
        return result;
    }
}
