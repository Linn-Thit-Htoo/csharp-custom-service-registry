﻿namespace CustomServiceRegistry.RegistryApi.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        builder
            .Configuration.SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile(
                $"appsettings.{builder.Environment.EnvironmentName}.json",
                optional: false,
                reloadOnChange: true
            )
            .AddEnvironmentVariables();

        builder
            .Services.AddControllers()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        builder
            .Services.AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddScoped<ITenantService, TenantService>()
            .AddScoped<IServiceRegistryService, ServiceRegistryService>()
            .AddScoped<IServiceLogService, ServiceLogService>()
            .AddScoped<IServiceDiscoveryService, ServiceDiscoveryService>()
            .AddTransient<CheckApiKeyMiddleware>()
            .AddTransient<RateLimiterMiddleware>()
            .AddHostedService<ActiveHealthCheckBackgroundService>()
            .Configure<AppSetting>(builder.Configuration)
            .AddHttpClient()
            .AddHttpContextAccessor()
            .AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjectionExtensions).Assembly);
            })
            .AddCors(options =>
            {
                options.AddPolicy(
                    "CORSPolicy",
                    builder =>
                        builder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .SetIsOriginAllowed((hosts) => true)
                );
            })
            .AddResponseCompression(opt =>
            {
                opt.EnableForHttps = true;
                opt.Providers.Add<GzipCompressionProvider>();
                opt.MimeTypes = ResponseCompressionDefaults.MimeTypes;
            })
            .Configure<GzipCompressionProviderOptions>(opt =>
            {
                opt.Level = System.IO.Compression.CompressionLevel.SmallestSize;
            });

        builder.Services.AddHealthChecks();

        return services;
    }

    public static IApplicationBuilder UseCheckApiKeyMiddleware(this WebApplication app)
    {
        return app.UseMiddleware<CheckApiKeyMiddleware>();
    }

    public static IApplicationBuilder UseRateLimiterMiddleware(this WebApplication app)
    {
        return app.UseMiddleware<RateLimiterMiddleware>();
    }
}
