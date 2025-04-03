using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.DeregisterService;
using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.RegisterService;
using CustomServiceRegistry.RegistryApi.Utils;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.Core
{
    public interface IServiceRegistryService
    {
        Task<Result<RegisterServiceResponse>> RegisterServiceAsync(RegisterServiceCommand command, CancellationToken cs = default);
        Task<Result<RegisterServiceResponse>> DeregisterAsync(DeregisterServiceCommand command, CancellationToken cs = default);
    }
}
