using BlueLagoon.Modules.Notifications.Core;
using BlueLagoon.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Notifications.Api;

internal class NotificationsModule : IModule
{
    public const string BasePath = "notification-module";

    public string Name { get; } = "Notifications";

    public string Path => BasePath;

    public void Register(IServiceCollection services)
    {
        services.AddCore();
    }

    public void Use(IApplicationBuilder app)
    {
    }
}
