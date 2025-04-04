using CustomServiceRegistry.RegistryApi.Configurations;
using CustomServiceRegistry.RegistryApi.Features.ServiceLog.Core;
using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.Core;
using CustomServiceRegistry.RegistryApi.Features.Tenant.Core;
using CustomServiceRegistry.RegistryApi.Middlewares;
using CustomServiceRegistry.RegistryApi.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

namespace CustomServiceRegistry.RegistryApi.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();

            builder.Services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjectionExtensions).Assembly);
            });

            builder.Services.AddScoped<ITenantService, TenantService>();
            builder.Services.AddScoped<IServiceRegistryService, ServiceRegistryService>();
            builder.Services.AddScoped<IServiceLogService, ServiceLogService>();
            builder.Services.AddTransient<CheckApiKeyMiddleware>();
            builder.Services.AddHostedService<ActiveHealthCheckBackgroundService>();
            builder.Services.Configure<AppSetting>(builder.Configuration);
            builder.Services.AddHealthChecks();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddCors(options =>
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
            });

            builder.Services.AddResponseCompression(opt =>
            {
                opt.EnableForHttps = true;
                opt.Providers.Add<GzipCompressionProvider>();
                opt.MimeTypes = ResponseCompressionDefaults.MimeTypes;
            });

            builder.Services.Configure<GzipCompressionProviderOptions>(opt =>
            {
                opt.Level = System.IO.Compression.CompressionLevel.SmallestSize;
            });

            return services;
        }

        public static IApplicationBuilder UseCheckApiKeyMiddleware(this WebApplication app)
        {
            return app.UseMiddleware<CheckApiKeyMiddleware>();
        }
    }
}
