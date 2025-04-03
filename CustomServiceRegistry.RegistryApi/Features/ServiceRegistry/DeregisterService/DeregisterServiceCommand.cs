using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.DeregisterService
{
    public class DeregisterServiceCommand : IRequest<Result<DeregisterServiceResponse>>
    {
        public Guid ServiceId { get; set; }
    }
}
