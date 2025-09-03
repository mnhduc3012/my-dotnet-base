using MyDotNetBase.Application.Abstractions.Emails;
using MyDotNetBase.Domain.Identity.Events;
using MyDotNetBase.Domain.Shared.ValueObjects;

namespace MyDotNetBase.Application.Identity.EventHandlers;

public sealed class OtpGeneratedDomainEventHandler : INotificationHandler<OtpGeneratedDomainEvent>
{
    private readonly IEmailSender _emailSender;
    private readonly ITemplateRenderer _templateRenderer;
    private readonly IUserRepository _userRepository;

    public OtpGeneratedDomainEventHandler(
        ITemplateRenderer templateRenderer,
        IEmailSender emailSender,
        IUserRepository userRepository)
    {
        _templateRenderer = templateRenderer;
        _emailSender = emailSender;
        _userRepository = userRepository;
    }

    public async Task Handle(OtpGeneratedDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(new Email(notification.Email), cancellationToken);

        var mailBody = await _templateRenderer.RenderAsync(
            "VERIFY_EMAIL",
            new
            {
                AppName = "MyDotNetBase",
                UserName = user?.FullName ?? notification.Email,
                Code = notification.Code,
                ExpiresMinutes = notification.ExpiresInMinutes
            },
            cancellationToken);

        await _emailSender.SendEmailAsync(
            notification.Email.ToString(),
            "Welcome to MyDotNetBase! Please verify your email",
            mailBody,
            cancellationToken);
    }
}
