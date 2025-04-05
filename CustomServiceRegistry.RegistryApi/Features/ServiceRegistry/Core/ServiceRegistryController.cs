using CustomServiceRegistry.RegistryApi.Features.Core;
using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.DeregisterService;
using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.RegisterService;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.Core;

[Route("api/[controller]")]
[ApiController]
public class ServiceRegistryController : BaseController
{
    private readonly ISender _sender;

    public ServiceRegistryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("RegisterService")]
    public async Task<IActionResult> RegisterService(
        RegisterServiceCommand command,
        CancellationToken cs
    )
    {
        var result = await _sender.Send(command, cs);
        return Content(result);
    }

    [HttpPost("DeregisterService")]
    public async Task<IActionResult> DeregisterService(
        DeregisterServiceCommand command,
        CancellationToken cs
    )
    {
        var result = await _sender.Send(command, cs);
        return Content(result);
    }
}
