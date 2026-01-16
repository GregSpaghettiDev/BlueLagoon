using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.Infrastructure.DAL.EfInterceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Shared.Infrastructure.DI.ServiceExtensions;

internal sealed class EfInterceptors : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<SaveChangesWithAuditInterceptor>();
    }
}
