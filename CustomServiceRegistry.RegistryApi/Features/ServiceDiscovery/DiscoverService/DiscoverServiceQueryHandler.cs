using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.DiscoverService
{
    public class DiscoverServiceQueryHandler : IRequestHandler<DiscoverServiceQuery, Result<DiscoverServiceResponse>>
    {
        public Task<Result<DiscoverServiceResponse>> Handle(DiscoverServiceQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
