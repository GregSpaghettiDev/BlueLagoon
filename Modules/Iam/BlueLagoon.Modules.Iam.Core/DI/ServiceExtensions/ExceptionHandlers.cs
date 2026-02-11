using BlueLagoon.Modules.Iam.Core.Exceptions.Handlers;
using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Core.DI.ServiceExtensions;

internal sealed class ExceptionHandlers : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandler<IamCoreExceptionHandler>();
    }
}
