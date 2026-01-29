using BlueLagoon.Shared.DevTools.Installers;
using BlueLagoon.Shared.Infrastructure.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BlueLagoon.Shared.Infrastructure.DI.WebApplicationExtensions;

internal sealed class WebApp : IMiddlewaresInstaller
{
    public int InstallOrder => 1;

    public void Install(WebApplication application)
    {
        if (application.Environment.IsDevelopment())
        {
            application.UseCors(options =>
            {
                options
                    .AllowAnyOrigin()
                    .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                    .WithExposedHeaders("Content-Disposition")
                    .AllowAnyHeader();
            });
        }
        application.UseStaticFiles();
        application.UseAntiforgery();
        application.UseAuthentication();
        application.UseAuthorization();
        if (application.Environment.IsDevelopment())
        {
            var settings = application.Configuration.Get<SwaggerSettings>(); 

            application.UseSwagger();
            application.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(settings.RelativeOpenApiPath, settings.Description);
                options.OAuthClientId(settings.OauthClientId);
                options.OAuthAppName(settings.Name);
                options.OAuthUsePkce();
            });
        }
    }
}