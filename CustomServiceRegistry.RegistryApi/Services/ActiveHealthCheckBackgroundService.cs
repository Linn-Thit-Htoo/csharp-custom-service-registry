global using Polly;

namespace CustomServiceRegistry.RegistryApi.Services;

public class ActiveHealthCheckBackgroundService : BackgroundService
{
    private readonly IMongoCollection<CentralRegistryCollection> _centralRegistryCollection;
    private readonly IMongoCollection<ServiceLogCollection> _serviceLogCollection;
    private readonly ILogger<CentralRegistryCollection> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly AppSetting _setting;

    public ActiveHealthCheckBackgroundService(
        ILogger<CentralRegistryCollection> logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<AppSetting> settings
    )
    {
        _centralRegistryCollection =
            CollectionNames.CentralRegistryCollection.GetCollection<CentralRegistryCollection>();
        _serviceLogCollection =
            CollectionNames.ServiceLogCollection.GetCollection<ServiceLogCollection>();
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _setting = settings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (true)
            {
                var scope = _serviceScopeFactory.CreateScope();
                var registryService =
                    scope.ServiceProvider.GetRequiredService<IServiceRegistryService>();

                var lst = await _centralRegistryCollection
                    .Find(x => true)
                    .ToListAsync(cancellationToken: stoppingToken);

                if (lst is not null && lst.Count > 0)
                {
                    foreach (var item in lst)
                    {
                        var logs = await _serviceLogCollection
                            .Find(x => x.TenantId == item.TenantId)
                            .ToListAsync(cancellationToken: stoppingToken);

                        if (logs is not null && logs.Count > 0)
                        {
                            if (logs.Count >= _setting.LogLimit)
                            {
                                var deleteFilter = Builders<ServiceLogCollection>.Filter.Eq(
                                    x => x.TenantId,
                                    item.TenantId
                                );
                                await _serviceLogCollection.DeleteManyAsync(
                                    deleteFilter,
                                    stoppingToken
                                );
                            }
                        }

                        var retryPolicy = Policy
                            .Handle<WebException>()
                            .Or<Exception>()
                            .RetryAsync(
                                _setting.RetryCount,
                                onRetry: async (exception, retryCount, context) =>
                                {
                                    _logger.LogError(
                                        $"Service: {item.ServiceId}, Tenant Id: {item.TenantId} Retry Count: {retryCount}, Reason: {exception}"
                                    );

                                    await _serviceLogCollection.InsertOneAsync(
                                        new ServiceLogCollection()
                                        {
                                            LogId = Guid.NewGuid(),
                                            IsSuccess = false,
                                            ErrorMessage = exception.ToString(),
                                            LogAt = DateTime.UtcNow,
                                            TenantId = item.TenantId,
                                            ServiceInfo = new CentralRegistryCollection()
                                            {
                                                ServiceId = item.ServiceId,
                                                HealthCheckUrl = item.HealthCheckUrl,
                                                HostName = item.HostName,
                                                Port = item.Port,
                                                Id = item.Id,
                                                Scheme = item.Scheme,
                                                ServiceName = item.ServiceName,
                                                TenantId = item.TenantId,
                                            },
                                        },
                                        cancellationToken: stoppingToken
                                    );
                                }
                            );

                        var fallbackPolicy = Policy<HttpResponseMessage>
                            .Handle<WebException>()
                            .Or<Exception>()
                            .FallbackAsync(
                                async (ct) =>
                                {
                                    _logger.LogError(
                                        $"All health checks failed for service: {item.ServiceId}. Service: {item.ServiceId} will be deregistered. Tenant Id: {item.TenantId}"
                                    );
                                    await registryService.DeregisterAsync(item.ServiceId, ct);

                                    await _serviceLogCollection.InsertOneAsync(
                                        new ServiceLogCollection()
                                        {
                                            LogId = Guid.NewGuid(),
                                            IsSuccess = false,
                                            ErrorMessage =
                                                $"All health checks failed. Service Id: {item.ServiceId}, Service Name: {item.ServiceName} will be deregistered from the registry.",
                                            LogAt = DateTime.UtcNow,
                                            TenantId = item.TenantId,
                                            ServiceInfo = new CentralRegistryCollection()
                                            {
                                                ServiceId = item.ServiceId,
                                                HealthCheckUrl = item.HealthCheckUrl,
                                                HostName = item.HostName,
                                                Port = item.Port,
                                                Id = item.Id,
                                                Scheme = item.Scheme,
                                                ServiceName = item.ServiceName,
                                                TenantId = item.TenantId,
                                            },
                                        },
                                        cancellationToken: stoppingToken
                                    );

                                    return new HttpResponseMessage(
                                        HttpStatusCode.ServiceUnavailable
                                    );
                                }
                            );

                        var policy = fallbackPolicy.WrapAsync(retryPolicy);
                        HttpClient httpClient =
                            scope.ServiceProvider.GetRequiredService<HttpClient>();

                        await policy.ExecuteAsync(
                            () =>
                                httpClient.GetAsync(
                                    item.HealthCheckUrl,
                                    cancellationToken: stoppingToken
                                )
                        );

                        await _serviceLogCollection.InsertOneAsync(
                            new ServiceLogCollection()
                            {
                                LogId = Guid.NewGuid(),
                                IsSuccess = true,
                                ErrorMessage = null,
                                LogAt = DateTime.UtcNow,
                                TenantId = item.TenantId,
                                ServiceInfo = new CentralRegistryCollection()
                                {
                                    ServiceId = item.ServiceId,
                                    HealthCheckUrl = item.HealthCheckUrl,
                                    HostName = item.HostName,
                                    Port = item.Port,
                                    Id = item.Id,
                                    Scheme = item.Scheme,
                                    ServiceName = item.ServiceName,
                                    TenantId = item.TenantId,
                                },
                            },
                            cancellationToken: stoppingToken
                        );

                        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
