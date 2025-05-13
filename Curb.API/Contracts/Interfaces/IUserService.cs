using Curb.API.Contracts.Dtos;

namespace Curb.API.Contracts.Interfaces;

/// <summary>
/// Контракт сервиса по работе с пользователями.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Регистрация пользователя.
    /// </summary>
    /// <param name="userRegister">Пользователь.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Зарегистрированный пользователь.</returns>
    Task<UserDto> RegisterAsync(UserRegisterDto userRegister, CancellationToken cancellationToken);

    /// <summary>
    /// Получение пользователя по его идентификатору в Telegram.
    /// </summary>
    /// <param name="userId">Идентификатор.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Пользователь или <see langword="null"/>, если пользователь не найден.</returns>
    Task<UserDto?> GetByUserIdAsync(long userId, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление свойств пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="propertyValues">Свойства пользователя в виде ключ - значение, где ключ - название свойства, значение - значение свойства.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Выполненная заадча.</returns>
    Task UpdateAsync(Guid id, Dictionary<string, object?> propertyValues, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UserDto?> GetUserByEmail(string email, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UserLoginDto?> GetUserLogin(string email, CancellationToken cancellationToken);
}
