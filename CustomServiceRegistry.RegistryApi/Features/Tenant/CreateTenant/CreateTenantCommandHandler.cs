using CustomServiceRegistry.RegistryApi.Features.Tenant.Core;
using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CustomServiceRegistry.RegistryApi.Features.Tenant.CreateTenant;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Result<CreateTenantResponse>>
{
    private readonly ITenantService _tenantService;

    public CreateTenantCommandHandler(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public async Task<Result<CreateTenantResponse>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        Result<CreateTenantResponse> result;
        
        if (string.IsNullOrWhiteSpace(request.ApplicationName))
        {
            result = Result<CreateTenantResponse>.Fail("Application Name is required.");
        }

        result = await _tenantService.CreateTenantAsync(request, cancellationToken);

        return result;
    }
}
