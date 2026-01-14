using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Shared.DevTools.Url;

public static class Extensions
{
    public static string GetAppUrl(this IServiceProvider serviceProvider)
    {
        var server = serviceProvider.GetRequiredService<IServer>();
        var addressesFeature = server.Features.Get<IServerAddressesFeature>();

        if (addressesFeature == null || !addressesFeature.Addresses.Any())
            throw new InvalidOperationException("Nie udało się pobrać url. Serwer nie ma żadnych aktywnych adresów.");

        return addressesFeature.Addresses.First();
    }
}