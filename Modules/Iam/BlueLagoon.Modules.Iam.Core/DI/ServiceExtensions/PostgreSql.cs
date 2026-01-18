using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.Infrastructure.DAL.EfInterceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Reader;

namespace BlueLagoon.Modules.Iam.Core.DI.ServiceExtensions;

internal class PostgreSql : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<IamDbContext>((serviceProvider, options) =>
        { 
            var interceptor = serviceProvider.GetRequiredService<SaveChangesWithAuditInterceptor>();
            options.UseNpgsql(
                configuration.GetConnectionString("IamDb"),
                x =>
                {
                    x.MigrationsAssembly(typeof(IamDbContext).Assembly.FullName);
                    x.MigrationsHistoryTable("__EFMigrationsHistory", "iam");
                });

            options.UseOpenIddict<Application, Authorization, Module, Token, BaseId>();
            options.AddInterceptors(interceptor);
        });
    }
}