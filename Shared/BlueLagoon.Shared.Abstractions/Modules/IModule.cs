using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace BlueLagoon.Shared.Abstractions.Modules;

public interface IModule
{
    string Name { get; }

    string Path { get; }

    void Register(IServiceCollection services);

    void Use(IApplicationBuilder app);
}
