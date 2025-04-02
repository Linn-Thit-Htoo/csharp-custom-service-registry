using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.RegisterService
{
    public class RegisterServiceCommandHandler : IRequestHandler<RegisterServiceCommand, Result<RegisterServiceResponse>>
    {
        public Task<Result<RegisterServiceResponse>> Handle(RegisterServiceCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
