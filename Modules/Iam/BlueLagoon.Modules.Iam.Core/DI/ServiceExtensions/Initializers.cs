using BlueLagoon.Modules.Iam.Core.Services;
using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Core.DI.ServiceExtensions;

internal class Initializers : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<Initializer>();
        services.AddHostedService<IamSeeder>();
    }
}
