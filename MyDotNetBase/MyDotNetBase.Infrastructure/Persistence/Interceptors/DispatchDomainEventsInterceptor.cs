using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyDotNetBase.Domain.Shared.DomainEvents;

namespace MyDotNetBase.Infrastructure.Persistence.Interceptors;

public sealed class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DispatchDomainEventsInterceptor(IMediator mediator) => _mediator = mediator;

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is DbContext context)
        {
            var aggregates = context.ChangeTracker
                .Entries<IHasDomainEvents>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Count != 0)
                .ToList();

            foreach (var aggregate in aggregates)
            {
                foreach (var domainEvent in aggregate.DomainEvents)
                {
                    await _mediator.Publish(domainEvent, cancellationToken);
                }
                aggregate.ClearDomainEvents();
            }
        }
        return result;
    }
}
