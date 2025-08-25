using MyDotNetBase.Application.Abstractions.Authentication;
using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Application.Abstractions.Messaging;
using MyDotNetBase.Domain.Shared.Entities;
using MyDotNetBase.Domain.User.Enitties;
using MyDotNetBase.Domain.User.Services;

namespace MyDotNetBase.Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(string Email, string FullName, string Password) : ICommand;

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
        var defaultRoleOrError = await _roleRepository.GetDefaultRoleAsync();
        if (defaultRoleOrError.IsFailure)
            return defaultRoleOrError;

        var passwordHash = _passwordHasher.Hash(request.Password);

        var userOrError = await User.RegisterAsync(
            _emailUniquenessChecker,
            request.FullName,
            request.Email,
            passwordHash,
            [defaultRoleOrError.Value]);

        if (userOrError.IsFailure)
            return userOrError;

        await _userRepository.AddAsync(userOrError.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
