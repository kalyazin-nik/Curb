using Curb.API.DataAccess.Domains;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Curb.API.Contracts.Interfaces;

/// <summary>
/// Репозиторий.
/// </summary>
/// <typeparam name="TContext">Экземпляр класса <see cref="DbContext"/>.</typeparam>
internal interface IRepository<TContext> where TContext : DbContext
{
    /// <summary>
    /// Добваление.
    /// </summary>
    /// <typeparam name="TEntity">Сущность хранимая в репозитории.</typeparam>
    /// <param name = "entity" > Сущность.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns></returns>
    Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity;

    /// <summary>
    /// Возвращает все сущности <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Сущность хранимая в репозитории.</typeparam>
    /// <returns>Все элементы сущности <typeparamref name="TEntity"/></returns>
    IQueryable<TEntity> GetAll<TEntity>() where TEntity : BaseEntity;

    /// <summary>
    /// Получение сущности <typeparamref name="TEntity"/> по идентификатору.
    /// </summary>
    /// <typeparam name="TEntity">Сущность хранимая в репозитории.</typeparam>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Сущность <typeparamref name="TEntity"/></returns>
    Task<TEntity> GetByIdAsync<TEntity>(Guid id, CancellationToken cancellationToken) where TEntity : BaseEntity;

    /// <summary>
    /// Возвращает сущности <typeparamref name="TEntity"/> согласно условию.
    /// </summary>
    /// <typeparam name="TEntity">Сущность хранимая в репозитории.</typeparam>
    /// <param name="predicate">Условие.</param>
    /// <returns></returns>
    IQueryable<TEntity> GetByPredicate<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;

    /// <summary>
    /// Удаление.
    /// </summary>
    /// <typeparam name="TEntity">Сущность хранимая в репозитории.</typeparam>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns></returns>
    Task RemoveAsync<TEntity>(Guid id, CancellationToken cancellationToken) where TEntity : BaseEntity;

    /// <summary>
    /// Удаление.
    /// </summary>
    /// <typeparam name="TEntity">Сущность хранимая в репозитории.</typeparam>
    /// <param name="entity">Сущность.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns></returns>
    Task RemoveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity;

    /// <summary>
    /// Обновление.
    /// </summary>
    /// <typeparam name="TEntity">Сущность хранимая в репозитории.</typeparam>
    /// <param name="entity">Сущность.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns></returns>
    Task UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity;

    /// <summary>
    /// Проверка на существование сущности в репозитории по идентификатору.
    /// </summary>
    /// <typeparam name="TEntity">Сущность хранимая в репозитории.</typeparam>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Вернет true, в случае если сущность будет найдена, иначе false.</returns>
    Task<bool> IsExistAsync<TEntity>(Guid id, CancellationToken cancellationToken) where TEntity : BaseEntity;
}
