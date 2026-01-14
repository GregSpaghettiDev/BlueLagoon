using Microsoft.Extensions.Configuration;

namespace BlueLagoon.Shared.DevTools.Configuration;

public static class Extensions
{
    public static T GetSettings<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var settings = new T();
        var section = configuration.GetSection(sectionName);
        section.Bind(settings);

        return settings;
    }

    public static TSettings GetSettings<TSettings>(this IConfiguration configuration)
        where TSettings : class, new()
    {
        TSettings settings = new();
        configuration.Bind(typeof(TSettings).Name, settings);

        return settings;
    }
}
