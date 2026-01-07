using System.Net.Mail;

namespace Common.Smtp.Abstractions;

public interface IMailClient
{
    string DefaultEmailSenderAddress { get; }

    void Send(MailMessage message);
}