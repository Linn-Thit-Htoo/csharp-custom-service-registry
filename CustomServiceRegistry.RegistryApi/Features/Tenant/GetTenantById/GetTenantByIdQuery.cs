using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.Tenant.GetTenantById
{
    public class GetTenantByIdQuery : IRequest<Result<GetTenantByIdResponse>>
    {
        public string TenantId { get; set; }
    }
}
