using System.Net;
using CustomServiceRegistry.RegistryApi.Constants;
using CustomServiceRegistry.RegistryApi.Extensions;
using CustomServiceRegistry.RegistryApi.Utils;

namespace CustomServiceRegistry.RegistryApi.Middlewares;

public class CheckApiKeyMiddleware : IMiddleware
{
    private readonly ILogger<CheckApiKeyMiddleware> _logger;

    public CheckApiKeyMiddleware(ILogger<CheckApiKeyMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        Result<object> result;
        string requestPath = context.Request.Path;

        if (GetWhiteListPaths().Any(x => requestPath.Equals(x)))
        {
            await next(context);
            return;
        }

        string apiKey = context.Request.Headers[ApplicationConstants.ApiKey].ToString();
        if (apiKey is null || apiKey.IsNullOrWhiteSpace())
        {
            result = Result<object>.NotFound("Api Key not found.");

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = ApplicationConstants.ContentTypeJson;
            await context.Response.WriteAsync(result.ToJson());

            return;
        }

        await next(context);
    }

    private List<string> GetWhiteListPaths()
    {
        return new List<string>() { "/api/Tenant/SaveTenant", "/api/ServiceLog/StreamLogs", "/api/ServiceDiscovery/DiscoverService" };
    }
}
