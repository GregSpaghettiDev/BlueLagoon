using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Installers
{
    public interface IServicesInstaller
    {
        void Intstall(IServiceCollection services, IConfiguration configuration);
    }
}
