using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CustomServiceRegistry.RegistryApi.Features.Tenant.CreateTenant;

public record CreateTenantCommand(string ApplicationName) : IRequest<Result<CreateTenantResponse>>;
