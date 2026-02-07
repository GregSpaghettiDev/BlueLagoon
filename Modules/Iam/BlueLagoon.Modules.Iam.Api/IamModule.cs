using BlueLagoon.Modules.Iam.Core;
using BlueLagoon.Shared.DevTools.Modules.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Api;

public sealed class IamModule : IModule
{
    public const string BasePath = "iam";

    public string Name { get; } = "Iam";

    public string Path => BasePath;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.InstallIamServices(configuration);
    }

    public void Use(IApplicationBuilder app)
    {
    }

    public void ConfigureAuthorization(AuthorizationOptions options)
    {
        options.ConfigureIamPolicies();
    }
}