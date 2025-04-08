using CustomServiceRegistry.RegistryApi.Features.Tenant.Core;
using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.Tenant.GetTenantById;

public class GetTenantByIdQueryHandler : IRequestHandler<GetTenantByIdQuery, Result<GetTenantByIdResponse>>
{
    private readonly ITenantService _tenantService;

    public GetTenantByIdQueryHandler(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public async Task<Result<GetTenantByIdResponse>> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        Result<GetTenantByIdResponse> result;

        if (string.IsNullOrWhiteSpace(request.TenantId))
        {
            result = Result<GetTenantByIdResponse>.Fail("Tenant Id is required.");
        }

        result = await _tenantService.GetTenantByIdAsync(request.TenantId, cancellationToken);

        return result;
    }
}
