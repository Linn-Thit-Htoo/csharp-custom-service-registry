namespace CustomServiceRegistry.RegistryApi.Constants;

public class ApplicationConstants
{
    public static string ApiKey { get; } = "X-Key";
    public static string ContentTypeJson { get; } = "application/json";
    public static string ContentTypeSSE { get; } = "text/event-stream";
    public static string SSECacheControl { get; } = "no-cache";
    public static string SSEConnectionKeppAlive { get; } = "keep-alive";
}
