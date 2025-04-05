using CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.DiscoverService;
using CustomServiceRegistry.RegistryApi.Utils;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.Core
{
    public interface IServiceDiscoveryService
    {
        Task<Result<DiscoverServiceResponse>> DiscoverServiceAsync(Guid id, CancellationToken cs = default);
    }
}
