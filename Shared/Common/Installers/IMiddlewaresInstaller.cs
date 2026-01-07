using Microsoft.AspNetCore.Builder;

namespace Common.Installers
{
    public interface IMiddlewaresInstaller
    {
        void Install(WebApplication application);
    }
}
