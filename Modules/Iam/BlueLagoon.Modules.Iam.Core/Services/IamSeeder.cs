using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.Dictionaries;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Shared.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.Services;

internal sealed class IamSeeder(IHostApplicationLifetime lifetime, IServiceProvider serviceProvider, ILogger<IamSeeder> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        lifetime.ApplicationStarted.Register(() =>
        {
            _ = Task.Run(async () =>
            {
                using var scope = serviceProvider.CreateScope();
                logger.LogInformation("Aplikacja w pełni uruchomiona. Rozpoczęcie importu endpointów modułu IAM oraz inicjalnych danych słownikowych.");

                var moduleService = scope.ServiceProvider.GetRequiredService<IModuleService>();
                var context = scope.ServiceProvider.GetRequiredService<IamDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                var iamModule = await scopeManager.FindByNameAsync(Scope.Iam.Name);
                iamModule ??= await scopeManager.CreateAsync(new OpenIddictScopeDescriptor { Name = Scope.Iam.Name, DisplayName = Scope.Iam.Description, Resources = { Scope.Resource } });

                var profileScopeName = nameof(OpenIddictConstants.Permissions.Scopes.Profile).ToLower();
                var profileScope = await scopeManager.FindByNameAsync(OpenIddictConstants.Permissions.Scopes.Profile);
                profileScope ??= await scopeManager.CreateAsync(new OpenIddictScopeDescriptor { Name = profileScopeName, DisplayName = profileScopeName, Resources = { Scope.Resource } });

                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var openApiPath = configuration.GetSection("iam:module:openApiPath");
                var iamBaseUrl = configuration.GetSection("iam:module:baseUrl");

                if (string.IsNullOrWhiteSpace(iamBaseUrl?.Value))
                {
                    logger.LogError("Nie podano adresu bazowego (baseUrl) modułu IAM w konfiguracji aplikacji.");
                    throw new InvalidOperationException("Nie podano adresu bazowego (baseUrl) modułu IAM w konfiguracji aplikacji.");
                }

                if (string.IsNullOrWhiteSpace(openApiPath?.Value))
                {
                    logger.LogError("Nie podano ścieżki do OpenAPI (openApiPath) modułu IAM w konfiguracji aplikacji.");
                    throw new InvalidOperationException("Nie podano ścieżki do OpenAPI (openApiPath) modułu IAM w konfiguracji aplikacji.");
                }

                Uri openApiUri = new(new(iamBaseUrl.Value), openApiPath.Value);

                ((Module)iamModule).SetBaseUrl(iamBaseUrl?.Value);
                ((Module)iamModule).SetOpenApiPath(openApiUri.ToString());
                var discoveredEndpoints = await moduleService.GetEndpointDefinitionsFromOpenApi(openApiUri.ToString(), ((Module)iamModule).Name);

                string endpointPath = null;
                try
                {
                   
                    foreach (var endpoint in discoveredEndpoints)
                    {
                        endpointPath = endpoint.Path;
                        var ep = RegisteredEndpoint.Create(((Module)iamModule).Name, new HttpMethod(endpoint.HttpMethod), endpoint.Path, endpoint.OperationId, endpoint.Summary);

                        await context.AddAsync(ep);
                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Błąd podczas dodawania endpointu {Endpoint} do bazy danych.", endpointPath);
                    throw;
                }

                if (!await roleManager.RoleExistsAsync(ValueObjects.Role.IamAdminRoleName))
                    await roleManager.CreateAsync(Role.Create(ValueObjects.Role.IamAdmin));

                var defaultSystemUser = scope.ServiceProvider.GetRequiredService<DefaultSystemUser>();

                var user = await userManager.FindByIdAsync(defaultSystemUser.Id.ToString());
                if (user is not null && !await userManager.IsInRoleAsync(user, ValueObjects.Role.IamAdminRoleName))
                    await userManager.AddToRoleAsync(user, ValueObjects.Role.IamAdminRoleName);
            });
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    => Task.CompletedTask;  
}
