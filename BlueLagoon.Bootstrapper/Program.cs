using BlueLagoon.Modules.Iam.Api;
using BlueLagoon.Shared.DevTools.Modules;
using BlueLagoon.Shared.DevTools.Modules.Abstractions;
using BlueLagoon.Shared.Infrastructure;
using DotNetEnv;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
    Env.Load(Path.Combine(builder.Environment.ContentRootPath, "..", "docker", ".env"));

builder.Configuration.AddEnvironmentVariables(prefix: "Blue_");
builder.Host.ConfigureModules();
builder.InstallInfrastructureBuilderProviders(builder.Configuration);

builder.Services.InstallInfrastructureServices(builder.Configuration);

IList<Assembly> assemblies = ModuleLoader.LoadAssemblies(builder.Configuration, "BlueLagoon.Modules.");
IList<IModule> modules = ModuleLoader.LoadModules(assemblies);

foreach (var module in modules)
    module.Register(builder.Services, builder.Configuration);

var app = builder.Build();
app.InstallInfrastructureMiddlewares();
app.MapRazorComponents<BlueLagoon.Modules.Iam.Api.Components.App>();
app.MapControllers();

app.Logger.LogInformation($"Loaded modules: {string.Join(", ", modules.Select(x => x.Name))}");

assemblies.Clear();
modules.Clear();

app.Run();