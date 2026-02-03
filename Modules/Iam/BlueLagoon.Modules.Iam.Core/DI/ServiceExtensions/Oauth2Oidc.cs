using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Shared.DevTools.Configuration;
using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace BlueLagoon.Modules.Iam.Core.DI.ServiceExtensions;

internal sealed class Oauth2Oidc : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {

        var oauthSettings = configuration.GetSettings<Oauth2OidcSettings>();
        var provider = services.BuildServiceProvider();
        var env = provider.GetRequiredService<IHostEnvironment>();

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

                if (env.IsProduction())
                {
                    var settings = configuration.GetSettings<Oauth2OidcSettings>();

                    options.AddSigningKey(new SymmetricSecurityKey(Convert.FromBase64String(settings.SigningKey)));

                    options.AddEncryptionKey(new SymmetricSecurityKey(Convert.FromBase64String(settings.EncryptionKey)));

                    options.UseAspNetCore()
                       .EnableEndSessionEndpointPassthrough()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableTokenEndpointPassthrough()
                       .EnableStatusCodePagesIntegration();
                }
                else
                {
                    options.AddDevelopmentEncryptionCertificate()
                           .AddDevelopmentSigningCertificate();

                    options.UseAspNetCore()
                       .EnableEndSessionEndpointPassthrough()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableTokenEndpointPassthrough()
                       .EnableStatusCodePagesIntegration()
                       .DisableTransportSecurityRequirement();//Tylko dla developmentu. Pozwala na żądania bez https.
                }

                options.SetAccessTokenLifetime(TimeSpan.FromMinutes(10));
                options.SetRefreshTokenLifetime(TimeSpan.FromMinutes(30));

            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });
    }
}