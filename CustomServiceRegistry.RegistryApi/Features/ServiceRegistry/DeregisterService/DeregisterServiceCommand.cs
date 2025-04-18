﻿namespace CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.DeregisterService;

public record DeregisterServiceCommand(Guid ServiceId)
    : IRequest<Result<DeregisterServiceResponse>>;
