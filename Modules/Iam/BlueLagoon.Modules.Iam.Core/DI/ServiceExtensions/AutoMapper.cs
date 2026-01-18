using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Core.DI.ServiceExtensions;

internal sealed class AutoMapper : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(cf => { }, typeof(IamDbContext).Assembly);
    }
}
