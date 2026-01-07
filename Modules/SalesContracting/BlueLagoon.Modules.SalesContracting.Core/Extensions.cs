using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlueLagoon.Modules.SalesContracting.Api")]
namespace BlueLagoon.Modules.SalesContracting.Core;


internal static class Extensions
{
    internal static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services;
    }
}