using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Shared.Infrastructure.DI.ServiceExtensions;

internal sealed class Settings : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        FrontendAppSettings frontendAppSettings = new();
        configuration.Bind(nameof(FrontendAppSettings), frontendAppSettings);
        services.AddSingleton(frontendAppSettings);

        Oauth2OidcSettings oauth2OidcSettings = new();
        configuration.Bind(nameof(Oauth2OidcSettings), oauth2OidcSettings);
        services.AddSingleton(oauth2OidcSettings);

        SwaggerSettings swaggerSettings = new();
        configuration.Bind(nameof(SwaggerSettings), swaggerSettings);
        services.AddSingleton(swaggerSettings);

        DefaultSystemUser defaultSystemUser = new();
        configuration.Bind(nameof(DefaultSystemUser), defaultSystemUser);
        services.AddSingleton(defaultSystemUser);
    }
}
