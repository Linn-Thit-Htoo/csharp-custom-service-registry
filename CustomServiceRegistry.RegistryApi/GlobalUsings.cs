﻿global using MongoDB.Bson;
global using MongoDB.Bson.Serialization.Attributes;
global using CustomServiceRegistry.RegistryApi.Configurations;
global using CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.Core;
global using CustomServiceRegistry.RegistryApi.Features.ServiceLog.Core;
global using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.Core;
global using CustomServiceRegistry.RegistryApi.Features.Tenant.Core;
global using CustomServiceRegistry.RegistryApi.Middlewares;
global using CustomServiceRegistry.RegistryApi.Services;
global using Microsoft.AspNetCore.ResponseCompression;
global using System.Text.Json;
global using CustomServiceRegistry.RegistryApi.Collections;
global using CustomServiceRegistry.RegistryApi.Features.ServiceRegistry.RegisterService;
global using CustomServiceRegistry.RegistryApi.Features.Tenant.CreateTenant;
global using CustomServiceRegistry.RegistryApi.Constants;
global using MongoDB.Driver;
global using CustomServiceRegistry.RegistryApi.Extensions;
global using Microsoft.AspNetCore.Mvc;
global using CustomServiceRegistry.RegistryApi.Features.ServiceDiscovery.DiscoverService;
global using CustomServiceRegistry.RegistryApi.Utils;
global using CustomServiceRegistry.RegistryApi.Features.Core;
global using MediatR;
