using MyDotNetBase.Domain.Users.ValueObjects;

namespace MyDotNetBase.Domain.Users.Services;

public interface IEmailUniquenessChecker
{
    Task<bool> IsUniqueEmail(Email email);
}
