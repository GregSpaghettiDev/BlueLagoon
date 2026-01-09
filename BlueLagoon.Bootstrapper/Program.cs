using BlueLagoon.Shared.DevTools.Modules;
using BlueLagoon.Shared.DevTools.Modules.Abstractions;
using BlueLagoon.Shared.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureModules();
builder.InstallInfrastructureBuilderProviders(builder.Configuration);

IList<Assembly> assemblies = ModuleLoader.LoadAssemblies(builder.Configuration, "BlueLagoon.Modules.");
IList<IModule> modules = ModuleLoader.LoadModules(assemblies);

builder.Services.InstallInfrastructureServices(builder.Configuration, modules);

foreach (var module in modules)
    module.Register(builder.Services);

var app = builder.Build();
app.InstallInfrastructureMiddlewares();

app.Logger.LogInformation($"Loaded modules: {string.Join(", ", modules.Select(x => x.Name))}");

assemblies.Clear();
modules.Clear();

app.Run();
