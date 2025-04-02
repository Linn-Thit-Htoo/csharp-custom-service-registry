using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.RegisterService
{
    public class RegisterServiceCommand : IRequest<Result<RegisterServiceResponse>>
    {
        public string ServiceName { get; set; }
        public string Scheme { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string HealthCheckUrl { get; set; }
    }
}
