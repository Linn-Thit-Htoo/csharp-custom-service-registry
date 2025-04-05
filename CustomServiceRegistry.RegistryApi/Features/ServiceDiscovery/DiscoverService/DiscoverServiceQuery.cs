using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.DiscoverService
{
    public class DiscoverServiceQuery : IRequest<Result<DiscoverServiceResponse>>
    {
        public Guid ServiceId { get; set; }
    }
}
