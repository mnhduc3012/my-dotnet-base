namespace MyDotNetBase.Domain.Shared.DomainEvents;

public abstract record DomainEventBase : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
