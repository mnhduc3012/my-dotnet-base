using MediatR;

namespace MyDotNetBase.Domain.Shared.Abstractions;
public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
