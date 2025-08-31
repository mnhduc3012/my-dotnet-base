using MyDotNetBase.Domain.Shared.Entities;
using MyDotNetBase.Domain.Users.Errors;
using System.Net.Mail;

namespace MyDotNetBase.Domain.Users.ValueObjects;

public sealed record Email(string Value)
{
    public static Result<Email> Create(string email)
    {
        if (!IsValidEmail(email))
            return EmailErrors.Invalid;

        return new Email(email);
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static implicit operator string(Email email) => email.Value;
}

