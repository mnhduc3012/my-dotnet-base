using MyDotNetBase.Domain.Shared.Aggregates;

namespace MyDotNetBase.Infrastructure.Persistence.Repositories;

public abstract class Repository<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    protected readonly ApplicationDbContext DbContext;
    protected Repository(ApplicationDbContext context) => DbContext = context;

    public abstract Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken);

    public void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }
    public void Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
    }
    public void Remove(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }
}
