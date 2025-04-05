using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.Tenant.GetTenantById
{
    public record GetTenantByIdQuery(string TenantId) : IRequest<Result<GetTenantByIdResponse>>;
}
