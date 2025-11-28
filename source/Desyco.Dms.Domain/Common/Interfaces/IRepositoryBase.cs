using System.Linq.Expressions;
using Desyco.Dms.Domain.Common.Base;

namespace Desyco.Dms.Domain.Common.Interfaces;

public interface IRepositoryBase<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    
    Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? filter = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Expression<Func<TEntity, object>>[]? includes = null,
        CancellationToken cancellationToken = default);
        
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
    
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);
}
