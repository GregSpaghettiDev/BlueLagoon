using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.Dictionaries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using BlueLagoon.Shared.Infrastructure.Settings;
namespace BlueLagoon.Modules.Iam.Core.Services;

internal sealed class Worker(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IamDbContext>();

        var appManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        var frontendAppSettings = scope.ServiceProvider.GetRequiredService<FrontendAppSettings>();

        if (await scopeManager.FindByNameAsync(Scope.Iam.Name) == null)
            await scopeManager.CreateAsync(new OpenIddictScopeDescriptor { Name = Scope.Iam.Name, DisplayName = Scope.Iam.Description });

        if (await appManager.FindByClientIdAsync(frontendAppSettings.Name) == null)
        {
            await appManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = frontendAppSettings.Name,
                DisplayName = frontendAppSettings.Description,
                RedirectUris = 
                { 
                    new Uri(frontendAppSettings.CallbackUri), 
                    new Uri("https://localhost:5000/swagger/oauth2-redirect.html") 
                },
                PostLogoutRedirectUris = { new Uri(frontendAppSettings.Uri) },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.EndSession,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles,
                    OpenIddictConstants.Permissions.Prefixes.Scope + Scope.Iam.Name
                },
                Requirements =
                {
                    OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange
                }
            });
        }

        if (await appManager.FindByClientIdAsync("swagger-ui") == null)
        {
            await appManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "swagger-ui",
                DisplayName = "Dokumentacja API (Swagger)",
                RedirectUris = { new Uri("https://localhost:5000/swagger/oauth2-redirect.html") },
                Permissions = 
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Prefixes.Scope + Scope.Iam.Name,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "notification",
                    OpenIddictConstants.Permissions.Prefixes.Scope + "sales-contracting"
                }
            });
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}