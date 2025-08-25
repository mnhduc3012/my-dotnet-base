using MediatR;

namespace MyDotNetBase.Domain.Shared.DomainEvents;
public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
