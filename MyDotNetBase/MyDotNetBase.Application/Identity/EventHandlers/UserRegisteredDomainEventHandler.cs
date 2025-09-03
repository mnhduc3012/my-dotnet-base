using MyDotNetBase.Application.Abstractions.Emails;
using MyDotNetBase.Application.Abstractions.Identity;
using MyDotNetBase.Domain.Identity.Entities;
using MyDotNetBase.Domain.Users.Events;

namespace MyDotNetBase.Application.Identity.EventHandlers;

public sealed class UserRegisteredDomainEventHandler : INotificationHandler<UserRegisteredDomainEvent>
{
    private readonly IEmailSender _emailSender;
    private readonly ITemplateRenderer _templateRenderer;
    private readonly IOtpGenerator _otpGenerator;
    private readonly IOtpRepository _otpRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserRegisteredDomainEventHandler(
        IEmailSender emailSender,
        ITemplateRenderer templateRenderer,
        IOtpGenerator otpGenerator,
        IOtpRepository otpRepository,
        IUnitOfWork unitOfWork)
    {
        _emailSender = emailSender;
        _templateRenderer = templateRenderer;
        _otpGenerator = otpGenerator;
        _otpRepository = otpRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        var code = _otpGenerator.GenerateOtp();

        var otp = Otp.Create(
            notification.Email,
            code);

        _otpRepository.Add(otp);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
