using CustomServiceRegistry.RegistryApi.Features.Tenant.GetTenantById;

namespace CustomServiceRegistry.RegistryApi.Features.Tenant.Core;

public interface ITenantService
{
    Task<Result<CreateTenantResponse>> CreateTenantAsync(
        CreateTenantCommand command,
        CancellationToken cs = default
    );
    Task<Result<GetTenantByIdResponse>> GetTenantByIdAsync(
        string tenantId,
        CancellationToken cancellationToken = default
    );
}
