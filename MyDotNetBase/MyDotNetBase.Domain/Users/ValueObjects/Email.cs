using MyDotNetBase.Domain.Shared.Entities;
using MyDotNetBase.Domain.User.Errors;
using System.Net.Mail;

namespace MyDotNetBase.Domain.User.ValueObjects;

public sealed record Email
{
    public string Value { get; private set; }
    private Email(string value) => Value = value;

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
