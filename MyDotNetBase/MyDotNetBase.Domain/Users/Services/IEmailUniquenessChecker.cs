using MyDotNetBase.Domain.Shared.ValueObjects;

namespace MyDotNetBase.Domain.Users.Services;

public interface IEmailUniquenessChecker
{
    Task<bool> IsUniqueEmail(Email email);
}
