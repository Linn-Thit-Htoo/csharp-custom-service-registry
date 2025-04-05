using CustomServiceRegistry.RegistryApi.Collections;
using CustomServiceRegistry.RegistryApi.Constants;
using CustomServiceRegistry.RegistryApi.Extensions;
using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.DeregisterService;
using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.RegisterService;
using CustomServiceRegistry.RegistryApi.Utils;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MongoDB.Driver;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.Core;

public class ServiceRegistryService : IServiceRegistryService
{
    private readonly IMongoCollection<CentralRegistryCollection> _centralRegistryCollection;
    private readonly IMongoCollection<ServiceLogCollection> _serviceLogCollection;

    public ServiceRegistryService()
    {
        _centralRegistryCollection = CollectionNames.CentralRegistryCollection.GetCollection<CentralRegistryCollection>();
        _serviceLogCollection = CollectionNames.ServiceLogCollection.GetCollection<ServiceLogCollection>();
    }

    public async Task<Result<RegisterServiceResponse>> RegisterServiceAsync(RegisterServiceCommand command, Guid tenantId, CancellationToken cs = default)
    {
        Result<RegisterServiceResponse> result;

        var item = await _centralRegistryCollection
            .Find(x => x.ServiceName.Trim().Equals(command.ServiceName.Trim(), StringComparison.CurrentCultureIgnoreCase))
            .FirstOrDefaultAsync(cancellationToken: cs);

        if (item is not null)
        {
            result = Result<RegisterServiceResponse>.Fail("Service Name already exists.");
            goto result;
        }

        await _centralRegistryCollection.InsertOneAsync(command.ToCollection(tenantId), cancellationToken: cs);
        result = Result<RegisterServiceResponse>.Success();

    result:
        return result;
    }

    public async Task<Result<DeregisterServiceResponse>> DeregisterAsync(DeregisterServiceCommand command, Guid tenantId, CancellationToken cs = default)
    {
        Result<DeregisterServiceResponse> result;

        var item = await _centralRegistryCollection
            .Find(x => x.ServiceId == command.ServiceId && x.TenantId == tenantId).SingleOrDefaultAsync(cancellationToken: cs);
        if (item is null)
        {
            result = Result<DeregisterServiceResponse>.Fail("Service not found.");
            goto result;
        }

        var filter = Builders<CentralRegistryCollection>.Filter.Eq(x => x.ServiceId, command.ServiceId);
        await _centralRegistryCollection.DeleteOneAsync(filter, cs);

        var serviceLogsDeleteFilter = Builders<ServiceLogCollection>
            .Filter
            .Eq(x => x.ServiceInfo.ServiceId, item.ServiceId);
        await _serviceLogCollection.DeleteManyAsync(serviceLogsDeleteFilter, cs);

        result = Result<DeregisterServiceResponse>.Success();

    result:
        return result;
    }

    public async Task DeregisterAsync(Guid id, CancellationToken cs = default)
    {
        var item = await _centralRegistryCollection
            .Find(x => x.ServiceId == id).SingleOrDefaultAsync(cancellationToken: cs) ?? throw new Exception($"Service Id: {id} not found in the service registry.");

        var filter = Builders<CentralRegistryCollection>.Filter.Eq(x => x.ServiceId, id);
        await _centralRegistryCollection.DeleteOneAsync(filter, cs);
    }
}
