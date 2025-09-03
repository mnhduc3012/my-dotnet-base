using MyDotNetBase.Application.Abstractions.Emails;
using MyDotNetBase.Domain.Identity.Events;

namespace MyDotNetBase.Application.Identity.EventHandlers;

public sealed class OtpGeneratedDomainEventHandler : INotificationHandler<OtpGeneratedDomainEvent>
{
    private readonly IEmailSender _emailSender;
    private readonly ITemplateRenderer _templateRenderer;

    public OtpGeneratedDomainEventHandler(ITemplateRenderer templateRenderer, IEmailSender emailSender)
    {
        _templateRenderer = templateRenderer;
        _emailSender = emailSender;
    }

    public async Task Handle(OtpGeneratedDomainEvent notification, CancellationToken cancellationToken)
    {
        var mailBody = await _templateRenderer.RenderAsync(
        "VERIFY_EMAIL.html",
            new
            {
                AppName = "MyDotNetBase",
                UserName = notification.FullName,
                Code = notification.Code,
                ExpiresMinutes = notification.ExpiresInMinutes
            });

        await _emailSender.SendEmailAsync(
            notification.Email.ToString(),
            "Welcome to MyDotNetBase! Please verify your email",
            mailBody);
    }
}
