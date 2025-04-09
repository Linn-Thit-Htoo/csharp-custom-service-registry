namespace CustomServiceRegistry.RegistryApi.Extensions;

public static class Extension
{
    public static string ToJson(this object obj) => JsonSerializer.Serialize(obj);

    public static T ToObject<T>(this string jsonStr) => JsonSerializer.Deserialize<T>(jsonStr)!;

    public static bool IsNullOrWhiteSpace(this string str) =>
        string.IsNullOrWhiteSpace(str) || string.IsNullOrEmpty(str);
}
