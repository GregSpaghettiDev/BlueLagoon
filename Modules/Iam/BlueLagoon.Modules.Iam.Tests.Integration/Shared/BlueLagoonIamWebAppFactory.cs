using BlueLagoon.Modules.Iam.Core.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Modules.Iam.Tests.Integration.Shared;

public sealed class BlueLagoonIamWebAppFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>, IAsyncDisposable
    where TEntryPoint : class
{
    public BlueLagoonIamWebAppFactory()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Testing.json")
            .AddJsonFile("module.iam.json")
            .Build();
    }

    public IConfiguration Configuration { get; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.UseConfiguration(Configuration);

        builder.UseTestServer();

        builder.ConfigureTestServices(services =>
        {
            services.Configure<TestServerOptions>(options => options.AllowSynchronousIO = true);
            RemoveDbContextIfExists<IamDbContext>(services);

        });
    }

    private void RemoveDbContextIfExists<TContext>(IServiceCollection services)
        where TContext : DbContext
    {
        var dbContextOptionsDescriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<TContext>));
        if (dbContextOptionsDescriptor != null)
            services.Remove(dbContextOptionsDescriptor);

        var contextDescriptor = services.SingleOrDefault(x => x.ServiceType == typeof(TContext));
        if (contextDescriptor != null)
            services.Remove(contextDescriptor);
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
    }
}
