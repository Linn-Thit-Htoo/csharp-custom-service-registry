
namespace CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.DiscoverService;

public record DiscoverServiceQuery(string ServiceName) : IRequest<Result<DiscoverServiceResponse>>;
