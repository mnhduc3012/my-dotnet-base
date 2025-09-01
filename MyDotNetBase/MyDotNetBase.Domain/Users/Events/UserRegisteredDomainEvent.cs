using MyDotNetBase.Domain.Shared.Abstractions;
using MyDotNetBase.Domain.Users.ValueObjects;

namespace MyDotNetBase.Domain.Users.Events;

public sealed record UserRegisteredDomainEvent(
    UserId UserId,
    string FullName,
    Email Email
) : DomainEventBase;
