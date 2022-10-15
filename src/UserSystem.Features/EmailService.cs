using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using UserSystem.Models;
using MailKitSmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace UserSystem.Features;

public class EmailService:IEmailService
{
    private readonly AppSettings _appSettings;

    public EmailService(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }
    
    public void Send(string to, string subject, string text)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_appSettings.EmailFrom));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Plain) { Text = text };
              
              
        using var smtp = new MailKitSmtpClient();
        smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
        smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}