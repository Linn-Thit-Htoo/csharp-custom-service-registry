using CustomServiceRegistry.RegistryApi.Extensions;
using CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.Core;
using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.DiscoverService
{
    public class DiscoverServiceQueryHandler : IRequestHandler<DiscoverServiceQuery, Result<DiscoverServiceResponse>>
    {
        private readonly IServiceDiscoveryService _serviceDiscoveryService;

        public DiscoverServiceQueryHandler(IServiceDiscoveryService serviceDiscoveryService)
        {
            _serviceDiscoveryService = serviceDiscoveryService;
        }

        public async Task<Result<DiscoverServiceResponse>> Handle(DiscoverServiceQuery request, CancellationToken cancellationToken)
        {
            Result<DiscoverServiceResponse> result;

            if (request.ServiceName.IsNullOrWhiteSpace())
            {
                result = Result<DiscoverServiceResponse>.Fail("Id is required.");
                goto result;
            }

            result = await _serviceDiscoveryService.DiscoverServiceAsync(request.ServiceName, cancellationToken);

        result:
            return result;
        }
    }
}
