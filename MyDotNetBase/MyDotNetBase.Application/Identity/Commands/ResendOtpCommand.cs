
using MyDotNetBase.Application.Abstractions.Identity;
using MyDotNetBase.Domain.Identity.Entities;
using MyDotNetBase.Domain.Identity.Errors;
using MyDotNetBase.Domain.Shared.ValueObjects;
using MyDotNetBase.Domain.Users.Errors;

namespace MyDotNetBase.Application.Identity.Commands;

public sealed record ResendOtpCommand(
    string Email
) : ICommand;

public sealed class ResendOtpCommandHandler : ICommandHandler<ResendOtpCommand>
{
    private readonly IOtpRepository _otpRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOtpGenerator _otpGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public ResendOtpCommandHandler(
        IUserRepository userRepository,
        IOtpRepository otpRepository,
        IOtpGenerator otpGenerator,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _otpRepository = otpRepository;
        _otpGenerator = otpGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
    {
        var emailOrError = Email.Create(request.Email);
        if (emailOrError.IsFailure)
            return emailOrError.Error;

        var user = await _userRepository.GetByEmailAsync(emailOrError.Value, cancellationToken);
        if (user is null)
            return UserErrors.NotFound;
        if (user.IsEmailVerified)
            return UserErrors.EmailAlreadyVerified;

        var newOtpCode = _otpGenerator.GenerateOtp();

        var otp = await _otpRepository.GetByEmailAsync(emailOrError.Value, cancellationToken);

        if (otp is null)
        {
            otp = Otp.Create(
                 emailOrError.Value,
                 newOtpCode);

            _otpRepository.Add(otp);
        }
        else
        {
            if (otp.IsValid())
                return IdentityErrors.OtpAlreadySent;

            otp.ResendOtp(newOtpCode);

            _otpRepository.Update(otp);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
