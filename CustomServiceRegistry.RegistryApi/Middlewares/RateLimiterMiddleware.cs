using Microsoft.Extensions.Options;

namespace CustomServiceRegistry.RegistryApi.Middlewares;

public class RateLimiterMiddleware : IMiddleware
{
    private readonly IMongoCollection<TenantRateLimiterCollection> _tenantRateLimiterCollection;
    private readonly AppSetting _appSetting;

    public RateLimiterMiddleware(IOptions<AppSetting> appSetting)
    {
        _appSetting = appSetting.Value;
        _tenantRateLimiterCollection =
            CollectionNames.TenantRateLimiterCollection.GetCollection<TenantRateLimiterCollection>();
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        Result<object> result;
        string apiKey = context.Request.Headers[ApplicationConstants.ApiKey].ToString();

        if (apiKey.IsNullOrWhiteSpace())
        {
            await next(context);
            return;
        }

        var item = await _tenantRateLimiterCollection
            .Find(x => x.TenantId == Guid.Parse(apiKey))
            .SingleOrDefaultAsync();

        if (item is not null)
        {
            if (
                item.TotalRequest >= _appSetting.MaxRateLimitPerDayForEachTenant
                && item.CreatedAt.Day == DateTime.Now.Day
            )
            {
                result = Result<object>.Fail(
                    "Rate limit exceeded.",
                    HttpStatusCode.TooManyRequests
                );

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = ApplicationConstants.ContentTypeJson;
                await context.Response.WriteAsync(result.ToJson());

                return;
            }

            var updateFilter = Builders<TenantRateLimiterCollection>.Filter.Eq(
                x => x.TenantId,
                Guid.Parse(apiKey)
            );
            var update = Builders<TenantRateLimiterCollection>.Update.Inc(x => x.TotalRequest, 1);

            await _tenantRateLimiterCollection.UpdateOneAsync(updateFilter, update);
        }
        else
        {
            TenantRateLimiterCollection tenantRateLimiterCollection = new()
            {
                RateLimiterId = Guid.NewGuid(),
                TenantId = Guid.Parse(apiKey),
                CreatedAt = DateTime.Now,
                TotalRequest = 1,
            };
            await _tenantRateLimiterCollection.InsertOneAsync(tenantRateLimiterCollection);
        }

        await next(context);
    }
}
