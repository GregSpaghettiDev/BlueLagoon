using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlueLagoon.Modules.SalesContracting.Api")]
namespace BlueLagoon.Modules.SalesContracting.Application;

internal static class Extensions
{
    internal static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}