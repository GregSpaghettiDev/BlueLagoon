using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Mapping.DI.ServiceExtensions;

internal sealed class AutoMapper : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        throw new NotImplementedException();
    }
}
