global using CustomServiceRegistry.RegistryApi.Collections;
global using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.RegisterService;
global using CustomServiceRegistry.RegistryApi.Features.Tenant.CreateTenant;

namespace CustomServiceRegistry.RegistryApi.Extensions;

public static class Mapper
{
    public static TenantCollection ToCollection(this CreateTenantCommand command)
    {
        return new TenantCollection
        {
            ApplicationName = command.ApplicationName,
            TenantId = Guid.NewGuid(),
        };
    }

    public static CentralRegistryCollection ToCollection(
        this RegisterServiceCommand command,
        Guid tenantId
    )
    {
        return new CentralRegistryCollection
        {
            ServiceId = Guid.NewGuid(),
            TenantId = tenantId,
            HealthCheckUrl = command.HealthCheckUrl,
            HostName = command.HostName,
            Port = command.Port,
            Scheme = command.Scheme,
            ServiceName = command.ServiceName,
        };
    }
}
