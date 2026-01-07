using BlueLagoon.Modules.SalesContracting.Application;
using BlueLagoon.Modules.SalesContracting.Core;
using BlueLagoon.Modules.SalesContracting.Infrastructure;
using BlueLagoon.Modules.SalesContracting.Mapping;
using BlueLagoon.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.SalesContracting.Api;

internal class SalesContractingModule : IModule
{
    public const string BasePath = "sales-contracting";

    public string Name { get; } = "SalesContracting";

    public string Path => BasePath;

    public void Register(IServiceCollection services)
    {
        services.AddApplication();
        services.AddCore();
        services.AddInfrastructure();
        services.AddMapping();
    }

    public void Use(IApplicationBuilder app)
    {
    }
}
