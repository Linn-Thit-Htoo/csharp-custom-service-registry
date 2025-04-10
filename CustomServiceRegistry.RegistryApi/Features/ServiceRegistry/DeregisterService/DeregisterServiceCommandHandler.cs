using CustomServiceRegistry.RegistryApi.Utils;
using MediatR;

namespace CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.DeregisterService;

public class DeregisterServiceCommandHandler
    : IRequestHandler<DeregisterServiceCommand, Result<DeregisterServiceResponse>>
{
    private readonly IServiceRegistryService _serviceRegistryService;
    private readonly HttpContext _httpContext;

    public DeregisterServiceCommandHandler(
        IServiceRegistryService serviceRegistryService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _serviceRegistryService = serviceRegistryService;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public async Task<Result<DeregisterServiceResponse>> Handle(
        DeregisterServiceCommand request,
        CancellationToken cancellationToken
    )
    {
        return await _serviceRegistryService.DeregisterAsync(
            request,
            cancellationToken
        );
    }
}
