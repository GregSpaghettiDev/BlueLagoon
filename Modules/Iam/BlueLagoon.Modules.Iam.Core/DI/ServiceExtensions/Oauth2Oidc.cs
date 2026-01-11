using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.DevTools.Modules.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Core.DI.ServiceExtensions;

internal sealed class Oauth2Oidc : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration, IList<IModule> modules = null)
    {
        throw new NotImplementedException();
    }
}
