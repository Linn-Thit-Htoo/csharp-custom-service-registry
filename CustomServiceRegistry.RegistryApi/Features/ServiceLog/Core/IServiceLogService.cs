namespace CustomServiceRegistry.RegistryApi.Features.ServiceLog.Core;

public interface IServiceLogService
{
    Task<List<ServiceLogCollection>> GetLogCollectionAsync(
        Guid tenantId,
        CancellationToken cs = default
    );
}
