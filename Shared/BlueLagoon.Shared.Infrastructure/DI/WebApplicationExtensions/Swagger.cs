using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace BlueLagoon.Shared.Infrastructure.DI.WebApplicationExtensions;

internal sealed class Swagger : IMiddlewaresInstaller
{
    public int InstallOrder => 3;

    public void Install(WebApplication application)
    {
        if (application.Environment.IsDevelopment())
        {
            application.UseSwagger();
            application.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Blue Lagoon API V1");
                options.OAuthClientId("swagger-ui");
                options.OAuthAppName("Swagger UI");
                options.OAuthUsePkce();
            });
        }
    }
}