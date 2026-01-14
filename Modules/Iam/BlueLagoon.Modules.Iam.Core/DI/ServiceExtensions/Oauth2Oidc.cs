using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.Services;
using BlueLagoon.Shared.DevTools.Configuration;
using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.DevTools.Modules.Abstractions;
using BlueLagoon.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Core.DI.ServiceExtensions;

internal sealed class Oauth2Oidc : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration, IList<IModule> modules = null)
    {

        var oauthSettings = configuration.GetSettings<Oauth2OidcSettings>();

        services
            .AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<IamDbContext>();
            })
            .AddServer(options =>
            {
                options
                    .SetAuthorizationEndpointUris(oauthSettings.Endpoints.AuthorizationEndpoint)
                    .SetEndSessionEndpointUris(oauthSettings.Endpoints.LogoutEndpoint)
                    .SetTokenEndpointUris(oauthSettings.Endpoints.TokenEndpoint);

                options.AllowAuthorizationCodeFlow();
                options.AllowRefreshTokenFlow();

                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                       .EnableEndSessionEndpointPassthrough()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableTokenEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        services.AddHostedService<Worker>();
    }
}