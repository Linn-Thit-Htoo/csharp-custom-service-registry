using CustomServiceRegistry.RegistryApi.Constants;
using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.Core;
using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.DeregisterService;

public class DeregisterServiceCommandHandler : IRequestHandler<DeregisterServiceCommand, Result<DeregisterServiceResponse>>
{
    private readonly IServiceRegistryService _serviceRegistryService;
    private readonly HttpContext _httpContext;

    public DeregisterServiceCommandHandler(IServiceRegistryService serviceRegistryService, IHttpContextAccessor httpContextAccessor)
    {
        _serviceRegistryService = serviceRegistryService;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public async Task<Result<DeregisterServiceResponse>> Handle(DeregisterServiceCommand request, CancellationToken cancellationToken)
    {
        string? apiKey = _httpContext.Request.Headers[ApplicationConstants.ApiKey].ToString()
            ?? throw new Exception("Api Key cannot be nul.");

        return await _serviceRegistryService.DeregisterAsync(request, Guid.Parse(apiKey), cancellationToken);
    }
}
