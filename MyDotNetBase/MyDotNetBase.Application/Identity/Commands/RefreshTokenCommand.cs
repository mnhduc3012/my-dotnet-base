using MyDotNetBase.Application.Abstractions.Authentication;
using MyDotNetBase.Domain.Identity.Errors;
using MyDotNetBase.Domain.Shared.Results;

namespace MyDotNetBase.Application.Identity.Commands;

public sealed record RefreshTokenCommand(
    string RefreshToken
) : ICommand<RefreshTokenCommandResult>;

public sealed record RefreshTokenCommandResult(
    string AccessToken);

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .NotNull().WithMessage("Refresh token must not be null.");
    }
}

public sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenCommandResult>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RefreshTokenCommandHandler(
        ITokenProvider tokenProvider,
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _tokenProvider = tokenProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<RefreshTokenCommandResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(
            request.RefreshToken,
            cancellationToken);

        if (existingRefreshToken is null || existingRefreshToken.IsExpired())
            return IdentityErrors.InvalidRefreshToken;

        var user = await _userRepository.GetByIdAsync(
            existingRefreshToken.UserId,
            cancellationToken);

        if (user is null)
            return IdentityErrors.InvalidRefreshToken;

        var newAccessToken = _tokenProvider.GenerateAccessToken(user);
        // Optionally, you might want to invalidate the used refresh token and issue a new one
        // existingRefreshToken.Invalidate();
        // var newRefreshToken = RefreshToken.GenerateForUser(user.Id);
        // await _refreshTokenRepository.AddAsync(newRefreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshTokenCommandResult(newAccessToken);
    }
}