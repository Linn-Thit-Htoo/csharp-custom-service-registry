
using CustomServiceRegistry.RegistryApi.Collections;
using CustomServiceRegistry.RegistryApi.Constants;
using CustomServiceRegistry.RegistryApi.Extensions;
using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.Core;
using MongoDB.Driver;
using Polly;
using System.Net;

namespace CustomServiceRegistry.RegistryApi.Services
{
    public class ActiveHealthCheckBackgroundService : BackgroundService
    {
        private readonly IMongoCollection<CentralRegistryCollection> _centralRegistryCollection;
        private readonly ILogger<CentralRegistryCollection> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ActiveHealthCheckBackgroundService(ILogger<CentralRegistryCollection> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _centralRegistryCollection = CollectionNames.CentralRegistryCollection.GetCollection<CentralRegistryCollection>();
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                var scope = _serviceScopeFactory.CreateScope();
                var registryService = scope.ServiceProvider.GetRequiredService<IServiceRegistryService>();

                var lst = await _centralRegistryCollection.Find(x => true).ToListAsync(cancellationToken: stoppingToken);

                foreach (var item in lst)
                {
                    var retryPolicy = Policy
                    .Handle<WebException>()
                    .Or<Exception>()
                    .RetryAsync(
                        3,
                        onRetry: (exception, retryCount, context) =>
                        {
                            _logger.LogError($"Retry Count: {retryCount}, Reason: {exception}");
                        }
                    );

                    var fallbackPolicy = Policy<HttpResponseMessage>
                        .Handle<WebException>()
                        .Or<Exception>()
                        .FallbackAsync(
                            async (ct) =>
                            {
                                _logger.LogError(
                                    "All Health Checks Failed. Service will be deregistered."
                                );
                                await registryService.DeregisterAsync(item.ServiceId, ct);
                                return new HttpResponseMessage(
                                    System.Net.HttpStatusCode.ServiceUnavailable
                                );
                            }
                        );

                    var policy = fallbackPolicy.WrapAsync(retryPolicy);
                    HttpClient httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();

                    await policy.ExecuteAsync(() => httpClient.GetAsync(item.HealthCheckUrl));
                }
            }
        }
    }
}
