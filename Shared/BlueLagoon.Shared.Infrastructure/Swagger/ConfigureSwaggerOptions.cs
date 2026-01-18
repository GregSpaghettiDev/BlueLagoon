using BlueLagoon.Shared.DevTools.Url;
using BlueLagoon.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using OpenIddict.Abstractions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BlueLagoon.Shared.Infrastructure.Swagger;

public class ConfigureSwaggerOptions(IServiceProvider ServiceProvider) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        using var scope = ServiceProvider.CreateScope();

        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
        var oauthSettings = scope.ServiceProvider.GetRequiredService<Oauth2OidcSettings>();

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var scopes = scopeManager.ListAsync().ToBlockingEnumerable();
        var scopesDictionary = new Dictionary<string, string>();
        foreach (var s in scopes)
        {
            var name = scopeManager.GetNameAsync(s).GetAwaiter().GetResult();
            var displayName = scopeManager.GetDisplayNameAsync(s).GetAwaiter().GetResult();

            if (name != null)
                scopesDictionary.Add(name, displayName ?? name);
        }

        string baseUri = configuration["applicationUrl"] ?? scope.ServiceProvider.GetAppUrl();

        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(new Uri(baseUri), oauthSettings.Endpoints.AuthorizationEndpoint),
                    TokenUrl = new Uri(new Uri(baseUri), oauthSettings.Endpoints.TokenEndpoint),
                    Scopes = scopesDictionary
                }
            }
        });

        options.AddSecurityRequirement(doc =>
        {
            var requirement = new OpenApiSecurityRequirement
            {
                { new OpenApiSecuritySchemeReference("oauth2"), scopesDictionary.Keys.ToList() }
            };

            return requirement;
        });
    }
}