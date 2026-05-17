using BlueLagoon.Shared.DevTools.Configuration;
using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.Infrastructure.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace BlueLagoon.Shared.Infrastructure.DI.WebApplicationExtensions;

internal sealed class WebApp : IMiddlewaresInstaller
{
    public int InstallOrder => 1;

    public void Install(WebApplication application, IEnumerable<Assembly> loadedAssemblies = null)
    {
        if (application.Environment.IsDevelopment())
        {
            application.UseCors(options =>
            {
                options
                    .WithOrigins("http://localhost:4200")
                    .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                    .WithExposedHeaders("Content-Disposition")
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        }
        application.UseExceptionHandler();
        application.UseStatusCodePages();
        application.UseStaticFiles();
        application.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new EmbeddedFileProvider(this.GetType().Assembly, "BlueLagoon.Shared.Infrastructure.Swagger.js"),
            RequestPath = "/swagger-scripts"
        });

        if (application.Environment.IsDevelopment())
        {
            var settings = application.Configuration.GetSettings<SwaggerSettings>();

            if (settings is null)
                throw new InvalidOperationException("Brak ustawień SwaggerSettings podczas konfiguracji.");

            application.UseSwagger();
            application.UseSwaggerUI(options =>
            {
                //options.RoutePrefix = "swagger";
                options.SwaggerEndpoint(settings.RelativeOpenApiPath, settings.Description);
                options.OAuthClientId(settings.OauthClientId);
                options.OAuthAppName(settings.Name);
                options.OAuthUsePkce();
                options.InjectJavascript("../swagger-scripts/swagger-logout-patch.js");
            });
        }
        application.UseAntiforgery();
        application.UseAuthentication();
        application.UseAuthorization();
        application.UseExceptionHandler();
        application.UseStatusCodePages();
    }
}