namespace MyDotNetBase.Domain.Shared.Abstractions;

public abstract record DomainEventBase : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
