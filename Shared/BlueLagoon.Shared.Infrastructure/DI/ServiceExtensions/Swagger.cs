using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.Infrastructure.Swagger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BlueLagoon.Shared.Infrastructure.DI.ServiceExtensions;

internal sealed class Swagger : IServicesInstaller
{
    public void Intstall(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.DescribeAllParametersInCamelCase();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "BlueLagoon - modular API for telecom company",
                Version = "v1"
            });
        });
    }
}
