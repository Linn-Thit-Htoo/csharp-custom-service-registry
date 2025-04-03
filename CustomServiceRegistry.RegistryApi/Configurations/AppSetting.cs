namespace CustomServiceRegistry.RegistryApi.Configurations
{
    public class AppSetting
    {
        public Connectionstrings ConnectionStrings { get; set; }
        public int RetryCount { get; set; }
        public Logging Logging { get; set; }
    }

    public class Connectionstrings
    {
        public string MongoConnection { get; set; }
    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string MicrosoftAspNetCore { get; set; }
    }
}
