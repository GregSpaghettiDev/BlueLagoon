using BlueLagoon.Shared.DevTools.Modules.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Api;

public sealed class IamModule : IModule
{
    public const string BasePath = "iam-module";

    public string Name { get; } = "Iam";

    public string Path => BasePath;

    public void Register(IServiceCollection services)
    {
        services.Insall
    }

    public void Use(IApplicationBuilder app)
    {
    }
}
