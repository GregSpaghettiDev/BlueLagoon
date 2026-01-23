using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.Dictionaries;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Shared.DevTools.Uniqueidentifier;
using BlueLagoon.Shared.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
namespace BlueLagoon.Modules.Iam.Core.Services;

internal sealed class Initializer(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IamDbContext>();
        await context.Database.MigrateAsync(cancellationToken);

        var appManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var frontendAppSettings = scope.ServiceProvider.GetRequiredService<FrontendAppSettings>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var defaultSystemUser = scope.ServiceProvider.GetRequiredService<DefaultSystemUser>();
        var moduleService = scope.ServiceProvider.GetRequiredService<IModuleService>();
        
        if (!await userManager.Users.AnyAsync(x => x.Id == defaultSystemUser.Id.ToBaseId(), cancellationToken: cancellationToken))
        {
            var result = await userManager.CreateAsync(
            new User
            {
                Id = defaultSystemUser.Id,
                UserName = defaultSystemUser.Username,
                FirstName = defaultSystemUser.FirstName,
                LastName = defaultSystemUser.LastName,
                Email = defaultSystemUser.Email,
                EmailConfirmed = true
            }, defaultSystemUser.Password);

            if (!result.Succeeded)
                throw new InvalidOperationException($"Nieudana inicjalizacja użytkownika systemowego.", new Exception(result.Errors.FirstOrDefault()?.Description ?? string.Empty));
        }
            
        if (await appManager.FindByClientIdAsync(frontendAppSettings.OauthClientId) == null)
        {
            await appManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = frontendAppSettings.OauthClientId,
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