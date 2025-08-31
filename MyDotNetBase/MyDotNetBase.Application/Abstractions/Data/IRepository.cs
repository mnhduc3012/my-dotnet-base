using MyDotNetBase.Domain.Shared.Aggregates;

namespace MyDotNetBase.Application.Abstractions.Data;

public interface IRepository<TEntity, TId> 
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
