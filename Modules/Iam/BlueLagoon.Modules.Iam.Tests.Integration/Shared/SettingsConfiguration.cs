using Microsoft.Extensions.Configuration;

namespace BlueLagoon.Modules.Iam.Tests.Integration.Shared;

internal static class SettingsConfiguration
{
    public static IConfiguration InitConfiguration(string settingsFileName)
        => new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFileName, false, false)
                .AddEnvironmentVariables()
                .Build();

    public static TSettings FetchSettings<TSettings>(this IConfiguration configuration)
        where TSettings : class, new()
    {
        TSettings settings = new();
        configuration.Bind(typeof(TSettings).Name, settings);

        return settings;
    }
}