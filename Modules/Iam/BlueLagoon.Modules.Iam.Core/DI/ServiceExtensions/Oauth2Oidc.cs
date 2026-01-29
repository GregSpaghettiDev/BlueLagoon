using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.Configuration;
using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Core.DI.ServiceExtensions;

internal sealed class Oauth2Oidc : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {

        var oauthSettings = configuration.GetSettings<Oauth2OidcSettings>();

        services
            .AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<IamDbContext>()
                       .ReplaceDefaultEntities<Application, Authorization, Module, Token, Guid>();
            })
            .AddServer(options =>
            {
                options
                    .SetAuthorizationEndpointUris(oauthSettings.Endpoints.AuthorizationEndpoint)
                    .SetEndSessionEndpointUris(oauthSettings.Endpoints.LogoutEndpoint)
                    .SetTokenEndpointUris(oauthSettings.Endpoints.TokenEndpoint);

                options.AllowAuthorizationCodeFlow();
                options.AllowRefreshTokenFlow();

                options.RequireProofKeyForCodeExchange();

                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                       .EnableEndSessionEndpointPassthrough()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableTokenEndpointPassthrough()
                       .EnableStatusCodePagesIntegration().DisableTransportSecurityRequirement();

                //var environment = configuration["ASPNETCORE_ENVIRONMENT"];
                //if (environment == "Development")
                //    options.DisableTransportSecurityRequirement();

            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });
    }
}