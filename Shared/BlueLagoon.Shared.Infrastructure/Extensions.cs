using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.DevTools.Modules.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using DtProvider = BlueLagoon.Shared.Infrastructure.DI.ServiceExtensions.DateTimeProvider;

[assembly: InternalsVisibleTo("BlueLagoon.Bootstrapper")]
namespace BlueLagoon.Shared.Infrastructure;

internal static class Extensions
{
    public static void InstallInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IList<IModule> modules)
    {
        foreach (var installer in InstallersFetcher.GetInstallers<DtProvider, IServicesInstaller>())
            installer.Intstall(services, configuration, modules);
    }

    public static void InstallInfrastructureMiddlewares(this WebApplication application)
    {
        var installers = InstallersFetcher.GetInstallers<DtProvider, IMiddlewaresInstaller>();
        installers = installers.OrderBy(x => x.InstallOrder).ToList();

        foreach (var installer in installers)
            installer.Install(application);
    }

    public static void InstallInfrastructureBuilderProviders(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        foreach (var installer in InstallersFetcher.GetInstallers<DtProvider, IBuilderProvidersInstaller>())
            installer.Install(builder, configuration);
    }
}