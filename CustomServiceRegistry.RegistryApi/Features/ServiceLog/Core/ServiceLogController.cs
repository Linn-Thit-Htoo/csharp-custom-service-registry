﻿namespace CustomServiceRegistry.RegistryApi.Features.ServiceLog.Core;

[Route("api/v1/[controller]")]
[ApiController]
public class ServiceLogController : BaseController
{
    private readonly IServiceLogService _serviceLogService;
    private readonly HttpContext _httpContext;

    public ServiceLogController(
        IServiceLogService serviceLogService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _serviceLogService = serviceLogService;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    [HttpGet("StreamLogs")]
    public async Task StreamLogs(string apikey, CancellationToken cs)
    {
        _httpContext.Response.Headers.ContentType = ApplicationConstants.ContentTypeSSE;
        _httpContext.Response.Headers.CacheControl = ApplicationConstants.SSECacheControl;
        _httpContext.Response.Headers.Connection = ApplicationConstants.SSEConnectionKeppAlive;

        var responseStream = _httpContext.Response.Body;

        while (!cs.IsCancellationRequested)
        {
            var lst = await _serviceLogService.GetLogCollectionAsync(Guid.Parse(apikey), cs);

            string jsonStr = JsonSerializer.Serialize(lst);

            var sseMessage = $"data: {jsonStr}\n\n";

            var bytes = Encoding.UTF8.GetBytes(sseMessage);
            await responseStream.WriteAsync(bytes, cs);
            await responseStream.FlushAsync(cs);

            await Task.Delay(1000, cs);
        }
    }
}
