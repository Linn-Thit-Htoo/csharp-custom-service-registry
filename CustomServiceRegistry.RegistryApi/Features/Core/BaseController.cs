global using CustomServiceRegistry.RegistryApi.Extensions;
global using Microsoft.AspNetCore.Mvc;

namespace CustomServiceRegistry.RegistryApi.Features.Core;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected IActionResult Content(object obj)
    {
        return Content(obj.ToJson(), "application/json");
    }
}
