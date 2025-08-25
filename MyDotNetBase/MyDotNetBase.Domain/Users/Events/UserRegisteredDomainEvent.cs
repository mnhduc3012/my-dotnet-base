using MyDotNetBase.Domain.Shared.DomainEvents;
using MyDotNetBase.Domain.User.ValueObjects;

namespace MyDotNetBase.Domain.User.Events;

public sealed record UserRegisteredDomainEvent(
    UserId UserId,
    string FullName,
    Email Email
) : DomainEventBase;
