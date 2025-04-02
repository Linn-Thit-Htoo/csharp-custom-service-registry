using CustomServiceRegistry.RegistryApi.Collections;
using CustomServiceRegistry.RegistryApi.Constants;
using CustomServiceRegistry.RegistryApi.Extensions;
using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.RegisterService;
using CustomServiceRegistry.RegistryApi.Utils;
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

        public async Task<Result<RegisterServiceResponse>> RegisterServiceAsync(RegisterServiceCommand command, CancellationToken cs = default)
        {
            Result<RegisterServiceResponse> result;

            var item = await _centralRegistryCollection
                .Find(x => x.ServiceName.Trim().ToLower() == command.ServiceName.Trim().ToLower())
                .FirstOrDefaultAsync(cancellationToken: cs);

            if (item is not null)
            {
                result = Result<RegisterServiceResponse>.Fail("Service Name already exists.");
                goto result;
            }

            result:
            return result;
        }
    }
}
