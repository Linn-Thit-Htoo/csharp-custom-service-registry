FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_HTTP_PORTS=80
ENV TZ=Asia/Yangon

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CustomServiceRegistry.RegistryApi.csproj", "./"]
RUN dotnet restore "CustomServiceRegistry.RegistryApi.csproj" --verbosity normal
COPY . .
WORKDIR "/src"
RUN dotnet build "CustomServiceRegistry.RegistryApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CustomServiceRegistry.RegistryApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomServiceRegistry.RegistryApi.dll"]