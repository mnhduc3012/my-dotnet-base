using Microsoft.EntityFrameworkCore.Diagnostics;
using MyDotNetBase.Application.Abstractions.Authentication;

namespace MyDotNetBase.Infrastructure.Persistence.Interceptors;

public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IUserContext _userContext;

    public AuditSaveChangesInterceptor(IUserContext userContext) => _userContext = userContext;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var userId = _userContext.UserId ?? "System";

        foreach (var entry in dbContext.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Metadata.FindProperty("CreatedAt") != null)
                    entry.CurrentValues["CreatedAt"] = DateTime.UtcNow;

                if (entry.Metadata.FindProperty("CreatedBy") != null)
                    entry.CurrentValues["CreatedBy"] = userId;
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Metadata.FindProperty("UpdatedAt") != null)
                    entry.CurrentValues["UpdatedAt"] = DateTime.UtcNow;

                if (entry.Metadata.FindProperty("UpdatedBy") != null)
                    entry.CurrentValues["UpdatedBy"] = userId;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
