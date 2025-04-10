using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.DeregisterService;
using CustomServiceRegistry.RegistryApi.Utils;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.Core;

public interface IServiceRegistryService
{
    Task<Result<RegisterServiceResponse>> RegisterServiceAsync(
        RegisterServiceCommand command,
        Guid tenantId,
        CancellationToken cs = default
    );
    Task<Result<DeregisterServiceResponse>> DeregisterAsync(
        DeregisterServiceCommand command,
        CancellationToken cs = default
    );
    Task DeregisterAsync(Guid id, CancellationToken cs = default);
}
