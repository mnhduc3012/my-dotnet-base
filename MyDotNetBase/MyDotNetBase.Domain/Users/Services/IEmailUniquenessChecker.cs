namespace MyDotNetBase.Domain.User.Services;

public interface IEmailUniquenessChecker
{
    Task<bool> IsUnique(string email);
}
