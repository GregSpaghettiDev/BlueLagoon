using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Shared.DevTools.Modules.Abstractions;

public interface IModule
{
    string Name { get; }

    string Path { get; }

    void Register(IServiceCollection services, IConfiguration configuration);

    void Use(IApplicationBuilder app);

    void ConfigureAuthorization(AuthorizationOptions options);
}
