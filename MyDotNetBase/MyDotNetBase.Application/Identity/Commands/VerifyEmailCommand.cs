using MyDotNetBase.Domain.Identity.Errors;
using MyDotNetBase.Domain.Shared.ValueObjects;
using MyDotNetBase.Domain.Users.Errors;

namespace MyDotNetBase.Application.Identity.Commands;

public sealed record VerifyEmailCommand(
    string Email,
    string Code
) : ICommand;

public sealed class VerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand>
{
    private readonly IOtpRepository _otpRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public VerifyEmailCommandHandler(IOtpRepository otpRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _otpRepository = otpRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var emailOrError = Email.Create(request.Email);
        if (emailOrError.IsFailure)
            return emailOrError.Error;

        var otp = await _otpRepository.GetByEmailAndCodeAsync(
            emailOrError.Value,
            request.Code,
            cancellationToken);
        if (otp is null)
            return IdentityErrors.InvalidOtp;

        if (!otp.IsValid())
            return IdentityErrors.InvalidOtp;

        var user = await _userRepository.GetByEmailAsync(
            emailOrError.Value,
            cancellationToken);
        if (user is null)
            return UserErrors.NotFound;

        otp.MarkAsUsed();
        user.VerifyEmail();

        _otpRepository.Update(otp);
        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}