using System.Linq.Expressions;
using Scrima.Core.Query;

namespace Desyco.Dms.Domain.Common.Interfaces;

public interface IRepositoryBase<TEntity, in TKey> where TEntity : EntityBase<TKey>
{
    Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Expression<Func<TEntity, object>>[]? includes = null,
        CancellationToken cancellationToken = default);

    Task<QueryResult<TEntity>> GetResultListAsync(QueryOptions queryOptions,
        Expression<Func<TEntity, string, bool>>? searchExpression = null,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);
}
