using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Common.Installers
{
    public interface IBuilderProvidersInstaller
    {
        void Install(WebApplicationBuilder builder, IConfiguration configuration);
    }
}