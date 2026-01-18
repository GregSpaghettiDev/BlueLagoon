using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Shared.Infrastructure.DI.ServiceExtensions;

internal sealed class HttpClient : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
    }
}
