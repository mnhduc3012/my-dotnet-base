namespace MyDotNetBase.Domain.User.Services;

public interface IEmailUniquenessChecker
{
    Task<bool> IsUniqueEmail(string email);
}
