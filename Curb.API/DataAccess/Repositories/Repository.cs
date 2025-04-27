using System.Linq.Expressions;
using Curb.API.Contracts.Interfaces;
using Curb.API.DataAccess.Domains;
using Microsoft.EntityFrameworkCore;

namespace Curb.API.DataAccess.Repositories;

/// <inheridoc />
internal class Repository<TEntity, TContext> : IRepository<TEntity, TContext>
    where TEntity : BaseEntity
    where TContext : DbContext
{
    protected TContext DbContext;
    protected DbSet<TEntity> DbSet;

    public Repository(TContext dBContext)
    {
        DbContext = dBContext;
        DbSet = DbContext.Set<TEntity>();
    }

    // <inheridoc />
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    // <inheridoc />
    public IQueryable<TEntity> GetAll()
    {
        return DbSet;
    }

    // <inheridoc />
    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbSet.Where(x => x.Id == id).FirstAsync(cancellationToken);
    }

    // <inheridoc />
    public IQueryable<TEntity> GetByPredicate(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet.Where(predicate);
    }

    // <inheridoc />
    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        DbSet.Remove(entity!);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    // <inheridoc />
    public async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    // <inheridoc />
    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        DbSet.Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> IsExistAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbSet.Where(x => x.Id == id).SingleOrDefaultAsync(cancellationToken) is not null;
    }
}
