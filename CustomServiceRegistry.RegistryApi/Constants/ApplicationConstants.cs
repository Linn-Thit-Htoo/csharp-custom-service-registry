namespace CustomServiceRegistry.RegistryApi.Constants;

public class ApplicationConstants
{
    public static string ApiKey { get; } = "X-Key";
    public static string DatabaseName { get; } = "ServiceRegistry";
    public static string LocalMongoConnectionString { get; } = "mongodb://localhost:27017";
    public static string ContainerizedMongoConnectionString { get; } = "mongodb://root:root@mongo:27017/?authSource=admin";
    public static string Environment { get; } = "ASPNETCORE_ENVIRONMENT";
    public static string DevelopmentEnvironment { get; } = "Development";
    public static string ContentTypeJson { get; } = "application/json";
    public static string ContentTypeSSE { get; } = "text/event-stream";
    public static string SSECacheControl { get; } = "no-cache";
    public static string SSEConnectionKeppAlive { get; } = "keep-alive";
}
