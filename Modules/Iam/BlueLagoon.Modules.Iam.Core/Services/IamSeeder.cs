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

                var module = await scopeManager.FindByNameAsync(Scope.Iam.Name);
                module ??= await scopeManager.CreateAsync(new OpenIddictScopeDescriptor { Name = Scope.Iam.Name, DisplayName = Scope.Iam.Description, Resources = { Scope.Resource } });

                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var openApiPath = configuration.GetSection("iam:module:openApiPath");
                var iamBaseUrl = configuration.GetSection("iam:module:baseUrl");

                if (string.IsNullOrWhiteSpace(iamBaseUrl?.Value))
                    throw new InvalidOperationException("Nie podano adresu bazowego (baseUrl) modułu IAM w konfiguracji aplikacji.");

                if (string.IsNullOrWhiteSpace(openApiPath?.Value))
                    throw new InvalidOperationException("Nie podano ścieżki do OpenAPI (openApiPath) modułu IAM w konfiguracji aplikacji.");

                Uri openApiUri = new(new(iamBaseUrl.Value), openApiPath.Value);

                ((Module)module).SetBaseUrl(iamBaseUrl?.Value);
                ((Module)module).SetOpenApiPath(openApiUri.ToString());
                var discoveredEndpoints = await moduleService.GetEndpointDefinitionsFromOpenApi(openApiUri.ToString(), ((Module)module).Name);

                foreach (var endpoint in discoveredEndpoints)
                {
                    var ep = RegisteredEndpoint.Create(((Module)module).Name, new HttpMethod(endpoint.HttpMethod), endpoint.Path, endpoint.OperationId, endpoint.Summary);

                    try
                    {
                        await context.AddAsync(ep);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Błąd podczas dodawania endpointu {Endpoint} do bazy danych.", endpoint.Path);
                        throw;
                    }

                    await context.SaveChangesAsync();
                }

                if (!await roleManager.RoleExistsAsync(ValueObjects.Role.IamAdmin))
                    await roleManager.CreateAsync(Role.Create(ValueObjects.Role.IamAdmin, ValueObjects.Role.IamAdminDisplayRoleName));

                var defaultSystemUser = scope.ServiceProvider.GetRequiredService<DefaultSystemUser>();
                var user = await context.Users.Where(x => x.Id == defaultSystemUser.Id).SingleOrDefaultAsync();
                if (user is not null && !await userManager.IsInRoleAsync(user, ValueObjects.Role.IamAdmin))
                    await userManager.AddToRoleAsync(user, ValueObjects.Role.IamAdmin);

            });
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    => Task.CompletedTask;  
}
