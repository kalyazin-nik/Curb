using System.Linq.Expressions;
using Curb.API.Contracts.Interfaces;
using Curb.API.DataAccess.Domains;
using Microsoft.EntityFrameworkCore;

namespace Curb.API.DataAccess.Repositories;

/// <summary>
/// Репозиторий по работе с базой данных.
/// </summary>
/// <typeparam name="TContext">Тип контекста базы данных.</typeparam>
/// <param name="dBContext">Объект контекста базы данных.</param>
internal class Repository<TContext>(TContext dBContext) : IRepository<TContext>
    where TContext : DbContext
{
    protected TContext DbContext = dBContext;

    // <inheridoc />
    public async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity
    {
        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    // <inheridoc />
    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : BaseEntity
    {
        return DbContext.Set<TEntity>();
    }

    // <inheridoc />
    public async Task<TEntity> GetByIdAsync<TEntity>(Guid id, CancellationToken cancellationToken) where TEntity : BaseEntity
    {
        return await DbContext.Set<TEntity>().Where(x => x.Id == id).FirstAsync(cancellationToken);
    }

    // <inheridoc />
    public IQueryable<TEntity> GetByPredicate<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity
    {
        return DbContext.Set<TEntity>().Where(predicate);
    }

    // <inheridoc />
    public async Task RemoveAsync<TEntity>(Guid id, CancellationToken cancellationToken) where TEntity : BaseEntity
    {
        var entity = await GetByIdAsync<TEntity>(id, cancellationToken);
        DbContext.Set<TEntity>().Remove(entity!);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    // <inheridoc />
    public async Task RemoveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity: BaseEntity
    {
        DbContext.Set<TEntity>().Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    // <inheridoc />
    public async Task UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity
    {
        DbContext.Set<TEntity>().Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> IsExistAsync<TEntity>(Guid id, CancellationToken cancellationToken) where TEntity : BaseEntity
    {
        return await DbContext.Set<TEntity>().Where(x => x.Id == id).SingleOrDefaultAsync(cancellationToken) is not null;
    }
}
