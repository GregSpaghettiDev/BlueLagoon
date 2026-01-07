using Common.Smtp.Abstractions;
using System.Net;
using System.Net.Mail;

namespace Common.Smtp;

internal sealed class MailClient : SmtpClient, IMailClient
{
    private readonly SmtpSettings _smtpSettings;

    public string DefaultEmailSenderAddress { get; private set; }

    public MailClient(SmtpSettings smtpSettings)
    {
        _smtpSettings = smtpSettings;
        DefaultEmailSenderAddress = _smtpSettings.User;
        Host = _smtpSettings.Host;
        Port = _smtpSettings.Port;
        DeliveryMethod = SmtpDeliveryMethod.Network;
        Credentials = new NetworkCredential(_smtpSettings.User, _smtpSettings.Password);
    }

    ~MailClient()
    {
        Dispose(true);
    }
}
