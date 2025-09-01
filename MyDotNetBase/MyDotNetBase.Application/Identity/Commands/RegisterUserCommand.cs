using MyDotNetBase.Application.Abstractions.Authentication;
using MyDotNetBase.Domain.Shared.Results;
using MyDotNetBase.Domain.Users.Entities;
using MyDotNetBase.Domain.Users.Services;

namespace MyDotNetBase.Application.Identity.Commands;

public sealed record RegisterUserCommand(
    string Email,
    string FullName,
    string Password
) : ICommand;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage($"Email không hợp lệ");
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailUniquenessChecker _emailUniquenessChecker;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IEmailUniquenessChecker emailUniquenessChecker,
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _emailUniquenessChecker = emailUniquenessChecker;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var defaultRole = await _roleRepository.GetDefaultRoleAsync(cancellationToken);
        if (defaultRole is null)
            return Error.NullValue;

        var passwordHash = _passwordHasher.Hash(request.Password);

        var userOrError = await User.RegisterAsync(
            _emailUniquenessChecker,
            request.FullName,
            request.Email,
            passwordHash,
            [defaultRole]);

        if (userOrError.IsFailure)
            return userOrError.Error;

        _userRepository.Add(userOrError.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

