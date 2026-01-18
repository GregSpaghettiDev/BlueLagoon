using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.Infrastructure.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Shared.Infrastructure.DI.ServiceExtensions;

internal sealed class AutoMapper : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(cfg => { }, typeof(BaseProfile).Assembly);
    }
}
