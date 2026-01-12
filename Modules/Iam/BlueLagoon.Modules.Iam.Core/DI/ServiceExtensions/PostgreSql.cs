using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.DAL.Abstractions;
using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.DevTools.Modules.Abstractions;
using BlueLagoon.Shared.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Core.DI.ServiceExtensions;

internal class PostgreSql : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration, IList<IModule> modules = null)
    {
        services.AddDbContextPool<IamDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("IamDb"), x => x.MigrationsAssembly(typeof(IamDbContext).Assembly.FullName));
            options.UseOpenIddict();
        });
        services.AddScoped<IIamDbContext>(provider => provider.GetRequiredService<IamDbContext>());
        services.AddScoped<DbContextProvider<IIamDbContext>>();
    }
}