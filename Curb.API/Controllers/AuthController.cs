using System.Net;
using Curb.API.Contracts.Dtos;
using Curb.API.Contracts.Interfaces;
using Curb.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Curb.API.Controllers;

/// <summary>
/// Аутентификация
/// </summary>
/// <param name="authService">Сервис по работе с аутентификацией и авторизацией.</param>
/// <param name="userService">Сервис по работе с пользователями.</param>
/// <param name="securityService"></param>
[ApiController]
[Route("api/auth")]
[Tags("Аутентификация")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class AuthController(IAuthService authService, IUserService userService, ISecurityService securityService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IUserService _userService = userService;
    private readonly ISecurityService _securityService = securityService;

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
        await _userService.UpdateAsync(user.Id, userRegister.MapToPropertyValues(refreshToken, 2), cancellationToken);

        return Ok(new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken });
    }

    /// <summary>
    /// Регистрация и авторизация нового пользователя.
    /// </summary>
    /// <param name="userRegister">Объект регистрации пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Токен авторизации.</returns>
    [HttpPost("register")]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(TokenDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegister, CancellationToken cancellationToken)
    {
        if (await _userService.GetUserByEmail(userRegister.Email!, cancellationToken) is not null)
        {
            return Conflict("Пользователь с такой электронной почтой уже существует.");
        }

        var user = await _userService.RegisterAsync(userRegister, cancellationToken);
        var accessToken = await _authService.GenerateAccessToken(user, cancellationToken);
        var refreshToken = await _authService.GenerateRefreshToken(cancellationToken);
        await _userService.UpdateAsync(user.Id, userRegister.MapToPropertyValues(refreshToken, 2), cancellationToken);

        return Ok(new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken });
    }

    /// <summary>
    /// Вход пользователя в систему.
    /// </summary>
    /// <param name="userLogin">Объект входа пользователя в систему.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Токен авторизации.</returns>
    [HttpPost("login")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(TokenDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Login(UserLoginDto userLogin, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserLogin(userLogin.Email!, cancellationToken);
        if (user is not null)
        {
            if (_securityService.IsPasswordVerified(userLogin.Password, user.Password))
            {
                var accessToken = await _authService.GenerateAccessToken(user.MapToUserDto(), cancellationToken);
                var refreshToken = await _authService.GenerateRefreshToken(cancellationToken);
                await _userService.UpdateAsync(user.Id, user.MapToPropertyValues(refreshToken, 2), cancellationToken);
                return Ok(new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken });
            }

            return Unauthorized("Неправильные логин или пароль.");
        }

        return NotFound("Пользователь не найден.");
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
        if (await _authService.AccessTokenIsValidAsync(Request.Headers.Authorization.ToString()["Bearer ".Length..].Trim(), cancellationToken)
            && await _authService.RefreshTokenIsValidAsync(user, cancellationToken))
        {
            var accessToken = await _authService.GenerateAccessToken(user, cancellationToken);
            return Ok(new TokenDto { AccessToken = accessToken, RefreshToken = user.RefreshToken });
        }

        return Unauthorized();
    }

    /// <summary>
    /// Закрытие сессии пользователя.
    /// </summary>
    /// <param name="user">Объект пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Успешное выполнение закрытия сессии.</returns>
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Logout([FromBody] UserDto user, CancellationToken cancellationToken)
    {
        await _authService.UserLogout(user, cancellationToken);
        return Ok();
    }
}