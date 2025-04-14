namespace CustomServiceRegistry.RegistryApi.Features.Tenant.CreateTenant;

public record CreateTenantCommand(string ApplicationName) : IRequest<Result<CreateTenantResponse>>;
