using Microsoft.Extensions.Options;
using MyDotNetBase.Application.Abstractions.Emails;
using System.Net;
using System.Net.Mail;

namespace MyDotNetBase.Infrastructure.Emails;

public class EmailSender : IEmailSender
{
    private readonly EmailConfiguration _emailConfiguration;
    public EmailSender(IOptions<EmailConfiguration> emailConfiguration)
    {
        _emailConfiguration = emailConfiguration.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort)
        {
            Credentials = new NetworkCredential(_emailConfiguration.Username, _emailConfiguration.Password),
            EnableSsl = true
        };

        using var message = new MailMessage
        {
            From = new MailAddress(_emailConfiguration.Username, _emailConfiguration.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(to);

        await client.SendMailAsync(message);
    }
}