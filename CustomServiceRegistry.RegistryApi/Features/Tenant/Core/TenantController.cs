using CustomServiceRegistry.RegistryApi.Constants;
using CustomServiceRegistry.RegistryApi.Features.Core;
using CustomServiceRegistry.RegistryApi.Features.Tenant.CreateTenant;
using CustomServiceRegistry.RegistryApi.Features.Tenant.GetTenantById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomServiceRegistry.RegistryApi.Features.Tenant.Core;

[Route("api/v1/[controller]")]
[ApiController]
public class TenantController : BaseController
{
    private readonly ISender _sender;
    private readonly HttpContext _context;

    public TenantController(ISender sender, IHttpContextAccessor httpContextAccessor)
    {
        _sender = sender;
        _context = httpContextAccessor.HttpContext!;
    }

    [HttpPost("SaveTenant")]
    public async Task<IActionResult> SaveTenant(CreateTenantCommand command, CancellationToken cs)
    {
        var result = await _sender.Send(command, cs);
        return Content(result);
    }

    [HttpGet("GetTenantInfoById")]
    public async Task<IActionResult> GetTenantById(CancellationToken cs)
    {
        string apiKey = _context.Request.Headers[ApplicationConstants.ApiKey]!;

        var query = new GetTenantByIdQuery(apiKey);
        var result = await _sender.Send(query, cs);

        return Content(result);
    }
}
