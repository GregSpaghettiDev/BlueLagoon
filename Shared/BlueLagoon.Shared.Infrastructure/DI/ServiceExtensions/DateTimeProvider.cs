using BlueLagoon.Shared.DevTools.DateAndTime;
using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.DevTools.Modules.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Shared.Infrastructure.DI.ServiceExtensions;

internal sealed class DateTimeProvider : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration, IList<IModule> modules = null)
    {
        services.AddDateTimeProvider();
    }
}
