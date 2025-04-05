using CustomServiceRegistry.RegistryApi.Features.Core;
using CustomServiceRegistry.RegistryApi.Features.Tenant.CreateTenant;
using CustomServiceRegistry.RegistryApi.Features.Tenant.GetTenantById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomServiceRegistry.RegistryApi.Features.Tenant.Core
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : BaseController
    {
        private readonly ISender _sender;

        public TenantController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("SaveTenant")]
        public async Task<IActionResult> SaveTenant(CreateTenantCommand command, CancellationToken cs)
        {
            var result = await _sender.Send(command, cs);
            return Content(result);
        }

        [HttpGet("GetTenantInfoById")]
        public async Task<IActionResult> GetTenantById(string id, CancellationToken cs)
        {
            var query = new GetTenantByIdQuery(id);
            var result = await _sender.Send(query, cs);

            return Content(result);
        }
    }
}
