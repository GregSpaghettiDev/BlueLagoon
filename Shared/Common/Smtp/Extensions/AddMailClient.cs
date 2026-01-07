using Common.Smtp.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Smtp.Extensions;

public static class MailClientExtension
{
    public static IServiceCollection AddMailClient(this IServiceCollection services, SmtpSettings smtpSettings)
    {
        services.AddScoped<IMailClient>(x => new MailClient(smtpSettings));
        return services;
    }
}
