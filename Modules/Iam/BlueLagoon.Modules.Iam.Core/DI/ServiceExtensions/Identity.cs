using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Core.DI.ServiceExtensions;

internal sealed class Identity : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = false; 
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<IamDbContext>()
        .AddDefaultTokenProviders();
    }
}
