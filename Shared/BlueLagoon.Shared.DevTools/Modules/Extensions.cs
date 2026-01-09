using BlueLagoon.Shared.DevTools.Api;
using BlueLagoon.Shared.DevTools.Modules.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlueLagoon.Shared.DevTools.Modules;

public static class Extensions
{
    public static IHostBuilder ConfigureModules(this IHostBuilder hostBuilder)
        => hostBuilder.ConfigureAppConfiguration((context, config) =>
        {
            IEnumerable<string> GetSettings(string pattern)
                => Directory.EnumerateFiles(context.HostingEnvironment.ContentRootPath, $"module.{pattern}.json", SearchOption.AllDirectories);
           
            var mainConfigSettings = GetSettings("*");
            var devConfigSettings = GetSettings($"*.{context.HostingEnvironment.EnvironmentName}");

            foreach (var settings in mainConfigSettings)
                config.AddJsonFile(settings);

            foreach (var settings in devConfigSettings)
                config.AddJsonFile(settings);
        });

    public static IServiceCollection AddModuleInfo(this IServiceCollection services, IList<IModule> modules)
    {
        var moduleInfoProvider = new ModuleInfoProvider();
        var moduleInfo = modules?.Select(x => new ModuleInfo(x.Name, x.Path)) ?? [];
        moduleInfoProvider.Modules.AddRange(moduleInfo);
        services.AddSingleton(moduleInfoProvider);

        return services;
    }

    public static void MapModuleInfo(this WebApplication endpoint)
    {
        endpoint.MapGet("modules", context =>
        {
            var moduleInfoProvider = context.RequestServices.GetRequiredService<ModuleInfoProvider>();
            return context.Response.WriteAsJsonAsync(moduleInfoProvider.Modules);
        });
    }

    public static IServiceCollection AddModuleControllers(this IServiceCollection services, IConfiguration configuration)
    {
        var disabledModules = new List<string>();
        foreach (var (key, value) in configuration.AsEnumerable())
        {
            if (!key.Contains(":module:enabled"))
                continue;

            if (!bool.Parse(value))
                disabledModules.Add(key.Split(":")[0]);
        }

        services.AddControllers()
                .ConfigureApplicationPartManager(manager =>
                {
                    var removedParts = new List<ApplicationPart>();
                    foreach (var disabledModule in disabledModules)
                    {
                        var parts = manager.ApplicationParts.Where(x => x.Name.Contains(disabledModule, StringComparison.InvariantCultureIgnoreCase));
                        removedParts.AddRange(parts);
                    }

                    foreach (var part in removedParts)
                        manager.ApplicationParts.Remove(part);

                    manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
                });
        return services;
    }
}
