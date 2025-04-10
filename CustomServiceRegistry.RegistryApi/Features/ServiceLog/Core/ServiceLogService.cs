using CustomServiceRegistry.RegistryApi.Extensions;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceLog.Core;

public class ServiceLogService : IServiceLogService
{
    private readonly IMongoCollection<ServiceLogCollection> _serviceLogCollection;

    public ServiceLogService()
    {
        _serviceLogCollection =
            CollectionNames.ServiceLogCollection.GetCollection<ServiceLogCollection>();
    }

    public async Task<List<ServiceLogCollection>> GetLogCollectionAsync(
        Guid tenantId,
        CancellationToken cs = default
    )
    {
        return await _serviceLogCollection.Find(x => true).ToListAsync(cancellationToken: cs);
    }
}
