using CustomServiceRegistry.RegistryApi.Extensions;

namespace CustomServiceRegistry.RegistryApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDependencies(builder);

        var app = builder.Build();

        app.UseResponseCompression();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("CORSPolicy");

        app.UseHealthChecks("/health");

        app.UseAuthorization();

        app.UseCheckApiKeyMiddleware();

        app.MapControllers();

        app.Run();
    }
}
