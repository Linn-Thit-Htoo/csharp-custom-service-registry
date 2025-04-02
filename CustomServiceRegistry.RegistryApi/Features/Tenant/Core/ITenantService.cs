using CustomServiceRegistry.RegistryApi.Features.Tenant.CreateTenant;
using CustomServiceRegistry.RegistryApi.Features.Tenant.GetTenantById;
using CustomServiceRegistry.RegistryApi.Utils;

namespace CustomServiceRegistry.RegistryApi.Features.Tenant.Core
{
    public interface ITenantService
    {
        Task<Result<CreateTenantResponse>> CreateTenantAsync(CreateTenantCommand command, CancellationToken cs = default);
        Task<Result<GetTenantByIdResponse>> GetTenantByIdAsync(string tenantId, CancellationToken cancellationToken = default);
    }
}
