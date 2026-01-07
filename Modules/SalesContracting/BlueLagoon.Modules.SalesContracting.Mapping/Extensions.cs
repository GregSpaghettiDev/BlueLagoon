using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlueLagoon.Modules.SalesContracting.Api")]
namespace BlueLagoon.Modules.SalesContracting.Mapping;

internal static class Extensions
{
    internal static IServiceCollection AddMapping(this IServiceCollection services)
    {
        return services;
    }
}