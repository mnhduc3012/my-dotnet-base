namespace MyDotNetBase.Domain.Users.Services;

public interface IEmailUniquenessChecker
{
    Task<bool> IsUniqueEmail(string email);
}
