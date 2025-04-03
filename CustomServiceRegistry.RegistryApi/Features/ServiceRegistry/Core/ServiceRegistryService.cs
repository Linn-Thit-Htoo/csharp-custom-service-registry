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

namespace CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.Core
{
    public class ServiceRegistryService : IServiceRegistryService
    {
        private readonly IMongoCollection<CentralRegistryCollection> _centralRegistryCollection;

        public ServiceRegistryService()
        {
            _centralRegistryCollection = CollectionNames.CentralRegistryCollection.GetCollection<CentralRegistryCollection>();
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

        public async Task<Result<RegisterServiceResponse>> DeregisterAsync(DeregisterServiceCommand command, CancellationToken cs = default)
        {
            Result<RegisterServiceResponse> result;

            var item = await _centralRegistryCollection.Find(x => x.ServiceId == command.ServiceId).SingleOrDefaultAsync(cancellationToken: cs);
            if (item is null)
            {
                result = Result<RegisterServiceResponse>.Fail("Service not found.");
                goto result;
            }

            var filter = Builders<CentralRegistryCollection>.Filter.Eq(x => x.ServiceId, command.ServiceId);
            await _centralRegistryCollection.DeleteOneAsync(filter, cs);

            result = Result<RegisterServiceResponse>.Success();

            result:
            return result;
        }
    }
}
