using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.DevTools.Modules.Abstractions;
using BlueLagoon.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Shared.Infrastructure.DI.ServiceExtensions;

internal sealed class Settings : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration, IList<IModule> modules = null)
    {
        FrontendAppSettings frontendAppSettings = new();
        configuration.Bind(nameof(FrontendAppSettings), frontendAppSettings);
        services.AddSingleton(frontendAppSettings);

        Oauth2OidcSettings oauth2OidcSettings = new();
        configuration.Bind(nameof(Oauth2OidcSettings), oauth2OidcSettings);
        services.AddSingleton(oauth2OidcSettings);
    }
}
