using System.Linq.Expressions;
using Desyco.Dms.Domain.Common.Interfaces;
using Desyco.Dms.Domain.Common.Base;
using Microsoft.EntityFrameworkCore;
using Scrima.Core.Query;
using Scrima.EntityFrameworkCore;

namespace Desyco.Dms.Infrastructure.Common.Repositories;

public abstract class RepositoryBase<TEntity, TKey>(ApplicationDbContext context) : IRepositoryBase<TEntity, TKey>
    where TEntity : EntityBase<TKey>
{
    protected readonly ApplicationDbContext Context = context;
    protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

    public Task<QueryResult<TEntity>> GetResultListAsync(
        QueryOptions queryOptions,
        Expression<Func<TEntity, string, bool>>? searchExpression = null,
        CancellationToken cancellationToken = default)
    {
        return DbSet.ToQueryResultAsync(queryOptions, searchExpression, cancellationToken: cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync([id], cancellationToken);
    }

    public virtual async Task<List<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Expression<Func<TEntity, object>>[]? includes = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = DbSet;
        
        // includes: [x => x.StatusId, x => x.Enrollments]);
        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync(cancellationToken);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(filter, cancellationToken);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual async Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            await DeleteAsync(entity, cancellationToken);
        }
    }
}
