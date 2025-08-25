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

    public DateTime CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }

    public void SetCreated(string userId)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = userId;
    }

    public void SetUpdated(string userId)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = userId;
    }
}


