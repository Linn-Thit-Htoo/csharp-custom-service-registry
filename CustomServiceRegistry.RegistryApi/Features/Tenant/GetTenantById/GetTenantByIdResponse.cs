namespace CustomServiceRegistry.RegistryApi.Features.Tenant.GetTenantById
{
    public class GetTenantByIdResponse
    {
        public Guid TenantId { get; set; }
        public string ApplicationName { get; set; }
    }
}
