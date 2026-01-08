using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Shared.DevTools.Modules.Abstractions;

public interface IModule
{
    string Name { get; }

    string Path { get; }

    void Register(IServiceCollection services);

    void Use(IApplicationBuilder app);
}
