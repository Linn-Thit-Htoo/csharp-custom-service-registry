using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.DeregisterService
{
    public class DeregisterServiceCommandHandler : IRequestHandler<DeregisterServiceCommand, Result<DeregisterServiceResponse>>
    {
        public Task<Result<DeregisterServiceResponse>> Handle(DeregisterServiceCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
