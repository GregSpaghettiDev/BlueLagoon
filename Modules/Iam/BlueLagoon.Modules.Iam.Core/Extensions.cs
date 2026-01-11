using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.DevTools.Modules.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Core;

internal static class Extensions
{
    public static void InstallIamServices(this IServiceCollection services, IConfiguration configuration, IList<IModule> modules)
    {
        foreach (var installer in InstallersFetcher.GetInstallers<Extensions, IServicesInstaller>())
            installer.Intstall(services, configuration, modules);
    }
}