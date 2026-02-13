using BlueLagoon.Modules.Iam.Core.Exceptions.Policies;
using BlueLagoon.Modules.Iam.Core.Exceptions.Policies.Abstractions;
using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Core.DI.ServiceExtensions;

internal class Policies : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IIdentityErrorPolicy, DuplicateUserNamePolicy>();
        services.AddScoped<IIdentityErrorPolicy, InvalidEmailPolicy>();
        services.AddScoped<IIdentityErrorPolicy, InvalidUserNamePolicy>();
        services.AddScoped<IIdentityErrorPolicy, PasswordNotMeetRequirementsPolicy>();
    }
}
