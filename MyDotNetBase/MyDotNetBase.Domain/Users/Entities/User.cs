using MyDotNetBase.Domain.Roles.Entities;
using MyDotNetBase.Domain.Shared.Abstractions;
using MyDotNetBase.Domain.Shared.Results;
using MyDotNetBase.Domain.Users.Errors;
using MyDotNetBase.Domain.Users.Events;
using MyDotNetBase.Domain.Users.Services;
using MyDotNetBase.Domain.Users.ValueObjects;
using System.Net.Mail;

namespace MyDotNetBase.Domain.Users.Entities;

public sealed class User : AggregateRoot<UserId>
{
    private readonly List<Role> _roles = [];
    public string Username { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public IReadOnlyList<Role> Roles => _roles.AsReadOnly();
    private User(UserId id) : base(id) { }

    public static async Task<Result<User>> CreateAsync(
        IEmailUniquenessChecker emailUniquenessChecker,
        string fullName,
        string email,
        string passwordHash,
        List<Role> roles)
    {
        if (roles.Count == 0)
            return UserErrors.NoRolesAssigned;

        var emailOrError = Email.Create(email);
        if (emailOrError.IsFailure)
            return emailOrError.Error;

        if (!await emailUniquenessChecker.IsUniqueEmail(emailOrError.Value))
            return UserErrors.DuplicateEmail(emailOrError.Value);

        var user = new User(UserId.New())
        {
            Username = emailOrError.Value,
            FullName = fullName,
            Email = emailOrError.Value,
            PasswordHash = passwordHash
        };

        foreach (var role in roles)
            user.AddRole(role);

        return user;
    }
    public static async Task<Result<User>> RegisterAsync(
        IEmailUniquenessChecker emailUniquenessChecker,
        string fullName,
        string email,
        string passwordHash,
        List<Role> roles)
    {
        var userOrError = await CreateAsync(emailUniquenessChecker, fullName, email, passwordHash, roles);
        if (userOrError.IsFailure)
            return userOrError.Error;

        var user = userOrError.Value;

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id, user.FullName, user.Email));
        return user;
    }

    public void AddRole(Role role)
    {
        if (!_roles.Contains(role))
            _roles.Add(role);
    }

    public void RemoveRole(Role role)
    {
        _roles.Remove(role);
    }
}
