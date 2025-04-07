using CustomServiceRegistry.RegistryApi.Collections;
using CustomServiceRegistry.RegistryApi.Configurations;
using CustomServiceRegistry.RegistryApi.Constants;
using CustomServiceRegistry.RegistryApi.Extensions;
using CustomServiceRegistry.RegistryApi.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Net;

namespace CustomServiceRegistry.RegistryApi.Middlewares
{
    public class RateLimiterMiddleware : IMiddleware
    {
        private readonly IMongoCollection<TenantRateLimiterCollection> _tenantReateLimiterCollection;
        private readonly AppSetting _appSetting;

        public RateLimiterMiddleware(IOptions<AppSetting> appSetting)
        {
            _appSetting = appSetting.Value;
            _tenantReateLimiterCollection = CollectionNames.TenantRateLimiterCollection.GetCollection<TenantRateLimiterCollection>();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Result<object> result;
            Guid apiKey = Guid.Parse(context.Request.Headers[ApplicationConstants.ApiKey].ToString());

            var item = await _tenantReateLimiterCollection
                .Find(x => x.TenantId == apiKey)
                .SingleOrDefaultAsync();

            if (item is not null)
            {
                if (item.TotalRequest >= _appSetting.MaxRateLimitPerDayForEachTenant)
                {
                    result = Result<object>.Fail("Rate limit exceeded.", HttpStatusCode.TooManyRequests);

                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result.ToJson());

                    return;
                }

                var updateFilter = Builders<TenantRateLimiterCollection>.Filter.Eq(x => x.TenantId, apiKey);
                var update = Builders<TenantRateLimiterCollection>.Update.Inc(x => x.TotalRequest, 1);

                await _tenantReateLimiterCollection.UpdateOneAsync(updateFilter, update);
            }
            else
            {
                TenantRateLimiterCollection tenantRateLimiterCollection = new()
                {
                    RateLimiterId = Guid.NewGuid(),
                    TenantId = apiKey,
                    CreatedAt = DateTime.Now,
                    TotalRequest = 1
                };
                await _tenantReateLimiterCollection.InsertOneAsync(tenantRateLimiterCollection);
            }

            await next(context);
        }
    }
}
