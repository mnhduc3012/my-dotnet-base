using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MyDotNetBase.Infrastructure.Persistence.Interceptors;

public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is not null)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Metadata.FindProperty("CreatedAt") != null)
                        entry.CurrentValues["CreatedAt"] = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    if (entry.Metadata.FindProperty("UpdatedAt") != null)
                        entry.CurrentValues["UpdatedAt"] = DateTime.UtcNow;
                }
            }
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
