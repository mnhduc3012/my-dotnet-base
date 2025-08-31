using Microsoft.EntityFrameworkCore.Diagnostics;
using MyDotNetBase.Domain.Shared.DomainEvents;
using MyDotNetBase.Infrastructure.Outbox;
using Newtonsoft.Json;

namespace MyDotNetBase.Infrastructure.Persistence.Interceptors;

public sealed class ConvertDomainEventsToOutboxMessageInterceptor
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var events = dbContext.ChangeTracker
            .Entries<IHasDomainEvents>()
            .Select(e => e.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.DomainEvents;
                aggregateRoot.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }),
                OccurredOn = domainEvent.OccurredOn
            })
            .ToList();

        dbContext.Set<OutboxMessage>().AddRange(events);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
