namespace MyDotNetBase.Domain.Shared.DomainEvents;

public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
