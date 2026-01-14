using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.DevTools.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Shared.Infrastructure.DI.ServiceExtensions;

internal sealed class Mvc : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleControllers(configuration);
    }
}
