using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.Core;
using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.RegisterService;

public class RegisterServiceCommandHandler
    : IRequestHandler<RegisterServiceCommand, Result<RegisterServiceResponse>>
{
    private readonly IServiceRegistryService _serviceRegistryService;
    private readonly HttpContext _httpContext;

    public RegisterServiceCommandHandler(
        IServiceRegistryService serviceRegistryService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _serviceRegistryService = serviceRegistryService;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public async Task<Result<RegisterServiceResponse>> Handle(
        RegisterServiceCommand request,
        CancellationToken cancellationToken
    )
    {
        string? apiKey =
            _httpContext.Request.Headers[ApplicationConstants.ApiKey].ToString()
            ?? throw new Exception("Api Key cannot be nul.");

        return await _serviceRegistryService.RegisterServiceAsync(
            request,
            Guid.Parse(apiKey),
            cancellationToken
        );
    }
}
