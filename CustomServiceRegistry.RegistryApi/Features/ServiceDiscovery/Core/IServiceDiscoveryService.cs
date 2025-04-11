namespace CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.Core;

public interface IServiceDiscoveryService
{
    Task<Result<DiscoverServiceResponse>> DiscoverServiceAsync(
        string serviceName,
        CancellationToken cs = default
    );
}
