using Curb.API.Contracts.Dtos;

namespace Curb.API.Contracts.Interfaces;

/// <summary>
/// Контаркт сервиса по работе с аутентификацией и авторизацией пользователей.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Проверка истинности данных, полученных от Telegram.
    /// </summary>
    /// <param name="userRegister">Пользователь.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Возвращает <see langword="true"/> в случае успешной проверки, иначе <see langword="false"/>.</returns>
    Task<bool> VerifyTelegramData(UserRegisterDto userRegister, CancellationToken cancellationToken);

    /// <summary>
    /// Генерирование токена доступа для конкретного пользователя.
    /// </summary>
    /// <param name="user">Пользователь.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Токен доступа.</returns>
    Task<string> GenerateAccessToken(UserDto user, CancellationToken cancellationToken);

    /// <summary>
    /// Генерирование токена обновления.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Токен обновления.</returns>
    Task<string> GenerateRefreshToken(CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="validateIssuer"></param>
    /// <param name="validateAudience"></param>
    /// <param name="validateLifetime"></param>
    /// <returns></returns>
    Task<bool> AccessTokenIsValidAsync(string accessToken, CancellationToken cancellationToken, bool validateIssuer = true, bool validateAudience = true, bool validateLifetime = false);

    /// <summary>
    /// Проверка достоверности токена обновления конкретного пользователя.
    /// </summary>
    /// <param name="userDto">Пользователь.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Возвращает <see langword="true"/> в случае успешной проверки, иначе <see langword="false"/>.</returns>
    Task<bool> RefreshTokenIsValidAsync(UserDto userDto, CancellationToken cancellationToken);

    /// <summary>
    /// Закрытие сессии пользователя.
    /// </summary>
    /// <param name="user">Пользователь.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Выполненная задача.</returns>
    Task UserLogout(UserDto user, CancellationToken cancellationToken);
}
