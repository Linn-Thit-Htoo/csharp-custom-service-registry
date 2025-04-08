using System.Net;
using CustomServiceRegistry.RegistryApi.Collections;
using CustomServiceRegistry.RegistryApi.Configurations;
using CustomServiceRegistry.RegistryApi.Constants;
using CustomServiceRegistry.RegistryApi.Extensions;
using CustomServiceRegistry.RegistryApi.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CustomServiceRegistry.RegistryApi.Middlewares;

public class RateLimiterMiddleware : IMiddleware
{
    private readonly IMongoCollection<TenantRateLimiterCollection> _tenantReateLimiterCollection;
    private readonly AppSetting _appSetting;

    public RateLimiterMiddleware(IOptions<AppSetting> appSetting)
    {
        _appSetting = appSetting.Value;
        _tenantReateLimiterCollection =
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

        var item = await _tenantReateLimiterCollection
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

            await _tenantReateLimiterCollection.UpdateOneAsync(updateFilter, update);
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
            await _tenantReateLimiterCollection.InsertOneAsync(tenantRateLimiterCollection);
        }

        await next(context);
    }
}
