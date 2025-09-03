using MyDotNetBase.Domain.Shared.Abstractions;

namespace MyDotNetBase.Domain.Identity.Events;

public sealed record OtpGeneratedDomainEvent(
    string Email,
    string FullName,
    string Code,
    int ExpiresInMinutes
) : DomainEventBase;
