using System.ComponentModel.DataAnnotations;
using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.Tenant.CreateTenant;

public record CreateTenantCommand(string ApplicationName) : IRequest<Result<CreateTenantResponse>>;
