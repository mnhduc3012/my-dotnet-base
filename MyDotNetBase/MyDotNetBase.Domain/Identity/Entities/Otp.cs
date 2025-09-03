using MyDotNetBase.Domain.Identity.Events;
using MyDotNetBase.Domain.Shared.Abstractions;
using MyDotNetBase.Domain.Shared.ValueObjects;

namespace MyDotNetBase.Domain.Identity.Entities;

public sealed class Otp(Guid id) : AggregateRoot<Guid>(id)
{
    private const int ExpirationMinutes = 5;
    public Email Email { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public DateTime ExpiresOnUtc { get; private set; }
    public bool IsUsed { get; private set; }

    public static Otp Create(Email email, string code, string fullname)
    {
        var otp = new Otp(Guid.NewGuid())
        {
            Email = email,
            Code = code,
            ExpiresOnUtc = DateTime.UtcNow.AddMinutes(ExpirationMinutes),
            IsUsed = false
        };

        otp.RaiseDomainEvent(new OtpGeneratedDomainEvent(email, fullname, code, ExpirationMinutes));

        return otp;
    }

    public void MarkAsUsed() => IsUsed = true;
    public bool IsValid() => !IsUsed && DateTime.UtcNow < ExpiresOnUtc;

    public void ResendOtp(string fullname, string newCode)
    {
        Code = newCode;
        ExpiresOnUtc = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        IsUsed = false;

        RaiseDomainEvent(new OtpGeneratedDomainEvent(Email, fullname, newCode, ExpirationMinutes));
    }
}
