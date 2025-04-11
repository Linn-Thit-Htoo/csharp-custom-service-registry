using CustomServiceRegistry.RegistryApi.Features.Core;
using CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.DiscoverService;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.Core;

[Route("api/v1/[controller]")]
[ApiController]
public class ServiceDiscoveryController : BaseController
{
    private readonly ISender _sender;

    public ServiceDiscoveryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("DiscoverService")]
    public async Task<IActionResult> DiscoverService(string serviceName, CancellationToken cs)
    {
        var query = new DiscoverServiceQuery(serviceName);
        var result = await _sender.Send(query, cs);

        return Content(result);
    }
}
