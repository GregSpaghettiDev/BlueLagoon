using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlueLagoon.Modules.Notifications.Api")]
namespace BlueLagoon.Modules.Notifications.Core;

internal static class Extensions
{
    internal static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services;
    }
}