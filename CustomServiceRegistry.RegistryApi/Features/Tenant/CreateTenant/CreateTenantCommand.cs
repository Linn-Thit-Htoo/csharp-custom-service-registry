using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CustomServiceRegistry.RegistryApi.Features.Tenant.CreateTenant
{
    public class CreateTenantCommand : IRequest<Result<CreateTenantResponse>>
    {
        [Required]
        public string ApplicationName { get; set; }
    }
}
