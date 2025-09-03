namespace MyDotNetBase.Application.Abstractions.Emails;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string body);
}
