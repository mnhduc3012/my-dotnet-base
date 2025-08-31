using MyDotNetBase.Application.Abstractions.Authentication;
using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Identity.Entities;
using MyDotNetBase.Domain.Identity.Errors;
using MyDotNetBase.Domain.Users.Errors;

namespace MyDotNetBase.Application.Identity.Commands;

public sealed record LoginCommand(
    string Username,
    string Password
) : ICommand<LoginCommandResult>;

public sealed record LoginCommandResult(
    string AccessToken,
    string RefreshToken
);

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Tên tài khoản không được để trống");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Mật khẩu không được để trống");
    }
}

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginCommandResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenProvider tokenProvider,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginCommandResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user is null)
            return IdentityErrors.InvalidCredentials;

        if (!_passwordHasher.Verify(user.PasswordHash, request.Password))
            return IdentityErrors.InvalidCredentials;

        var accessToken = _tokenProvider.GenerateAccessToken(user);

        var refreshToken = RefreshToken.Create(user.Id, _tokenProvider.GenerateRefreshToken());
        _refreshTokenRepository.Add(refreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginCommandResult(accessToken, refreshToken.Token);
    }
}
