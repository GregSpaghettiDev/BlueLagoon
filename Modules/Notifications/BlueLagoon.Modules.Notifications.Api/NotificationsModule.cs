using BlueLagoon.Modules.Notifications.Core;
using BlueLagoon.Shared.DevTools.Modules.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Notifications.Api;

public sealed class NotificationsModule : IModule
{
    public const string BasePath = "notifications";

    public string Name { get; } = "Notifications";

    public string Path => BasePath;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
    }

    public void Use(IApplicationBuilder app)
    {
    }

    public void ConfigureAuthorization(AuthorizationOptions options)
    {
    }
}
