using MyDotNetBase.Domain.Users.Events;

namespace MyDotNetBase.Application.Users.EventHandlers;

public sealed class UserRegisteredDomainEventHandler
    : INotificationHandler<UserRegisteredDomainEvent>
{
    public Task Handle(
        UserRegisteredDomainEvent notification,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
