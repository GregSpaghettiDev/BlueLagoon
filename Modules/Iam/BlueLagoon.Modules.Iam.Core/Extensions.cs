using BlueLagoon.Modules.Iam.Core.DI.ServiceExtensions;
using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlueLagoon.Modules.Iam.Api")]
namespace BlueLagoon.Modules.Iam.Core;

internal static class Extensions
{
    public static void InstallIamServices(this IServiceCollection services, IConfiguration configuration)
    {
        foreach (var installer in InstallersFetcher.GetInstallers<Oauth2Oidc, IServicesInstaller>())
            installer.Intstall(services, configuration);
    }
}