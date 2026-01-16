using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Shared.Infrastructure.DI.ServiceExtensions;

internal sealed class HttpContext : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
    }
}
