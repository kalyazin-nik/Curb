using System.Net;
using Curb.API.Contracts.Dtos;
using Curb.API.Contracts.Interfaces;
using Curb.API.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Curb.API.Controllers;

/// <summary>
/// Аутентификация
/// </summary>
/// <param name="authService">Сервис по работе с аутентификацией и авторизацией.</param>
/// <param name="userService">Сервис по работе с пользователями.</param>
[ApiController]
[Route("api/auth")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class AuthController(IAuthService authService, IUserService userService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IUserService _userService = userService;

    /// <summary>
    /// Авторизация пользователя после аутентификации через Telegram.
    /// </summary>
    /// <param name="userRegister">Объект регистрации пользователя Telegram.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Токен авторизации.</returns>
    [HttpGet("telegram-callback")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(TokenDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> TelegramCallback([FromQuery] UserRegisterDto userRegister, CancellationToken cancellationToken)
    {
        if (!await _authService.VerifyTelegramData(userRegister, cancellationToken))
        {
            return Unauthorized("Ошибка проверки данных.");
        }

        var user = await _userService.GetByUserIdAsync(long.Parse(userRegister.Id), cancellationToken);
        user ??= await _userService.RegisterAsync(userRegister, cancellationToken);
        var accessToken = await _authService.GenerateAccessToken(user, cancellationToken);
        var refreshToken = await _authService.GenerateRefreshToken(cancellationToken);

        user.RefreshToken = refreshToken;
        await _userService.UpdateAsync(user, userRegister.MapToPropertyValues(), cancellationToken);

        return Ok(new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken });
    }

    /// <summary>
    /// Обновление токена доступа.
    /// </summary>
    /// <param name="user">Объект пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Токен авторизации.</returns>
    [HttpPut("refresh-token")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(TokenDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> RefreshToken([FromBody] UserDto user, CancellationToken cancellationToken)
    {
        if (await _authService.RefreshTokenIsValidAsync(user, cancellationToken))
        {
            var accessToken = await _authService.GenerateAccessToken(user, cancellationToken);
            return Ok(new TokenDto { AccessToken = accessToken });
        }

        return Unauthorized();
    }

    /// <summary>
    /// Закрытие сессии пользователя.
    /// </summary>
    /// <param name="user">Объект пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Успешное выполнение закрытия сессии.</returns>
    [HttpPost("logout")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Logout([FromBody] UserDto user, CancellationToken cancellationToken)
    {
        await _authService.UserLogout(user, cancellationToken);
        return Ok();
    }
}