using MyDotNetBase.Domain.Shared.Auditing;
using MyDotNetBase.Domain.Shared.DomainEvents;
using MyDotNetBase.Domain.Shared.Entities;

namespace MyDotNetBase.Domain.Shared.Aggregates;

public abstract class AggregateRoot<TId> : Entity<TId>, IAuditable
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot(TId id) : base(id) { }

    protected void RaiseDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);
    public void ClearDomainEvents() => _domainEvents.Clear();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; } = null;
    public DateTime? UpdatedAt { get; set; } = null;
    public string? UpdatedBy { get; set; } = null;
}


