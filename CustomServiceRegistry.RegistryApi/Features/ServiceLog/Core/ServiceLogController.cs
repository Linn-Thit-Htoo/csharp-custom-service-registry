using CustomServiceRegistry.RegistryApi.Constants;
using CustomServiceRegistry.RegistryApi.Features.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceLog.Core
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceLogController : BaseController
    {
        private readonly IServiceLogService _serviceLogService;
        private readonly HttpContext _httpContext;

        public ServiceLogController(IServiceLogService serviceLogService, IHttpContextAccessor httpContextAccessor)
        {
            _serviceLogService = serviceLogService;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        [HttpGet("StreamLogs")]
        public async Task StreamLogs(CancellationToken cs)
        {
            string apiKey = _httpContext.Request.Headers[ApplicationConstants.ApiKey].ToString();

            _httpContext.Response.Headers.Append("Content-Type", "text/event-stream");

            while (!cs.IsCancellationRequested)
            {
                var lst = await _serviceLogService.GetLogCollectionAsync(Guid.Parse(apiKey), cs);

                await JsonSerializer.SerializeAsync(_httpContext.Response.Body, lst);
                await _httpContext.Response.Body.FlushAsync();
            }
        }
    }
}
