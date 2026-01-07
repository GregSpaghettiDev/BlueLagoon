using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlueLagoon.Modules.SalesContracting.Api")]
namespace BlueLagoon.Modules.SalesContracting.Infrastructure;

internal static class Extensions
{
    internal static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services;
    }
}