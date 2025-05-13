using Microsoft.AspNetCore.Mvc;

namespace Curb.API.Contracts.Dtos;

/// <summary>
/// Регистрация пользователя Telegram.
/// </summary>
public class UserRegisterDto
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    [FromQuery(Name = "id")]
    public string? Id { get; set; } = null!;

    /// <summary>
    /// Прозвище.
    /// </summary>
    [FromQuery(Name = "username")]
    public string? Username { get; set; }

    /// <summary>
    /// Имя.
    /// </summary>
    [FromQuery(Name = "first_name")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Фамилия.
    /// </summary>
    [FromQuery(Name = "last_name")]
    public string? LastName { get; set; }

    /// <summary>
    /// Электронная почта.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Дата аутентификации.
    /// </summary>
    [FromQuery(Name = "auth_date")]
    public string? AuthDate { get; set; } = null!;

    /// <summary>
    /// Url путь к фото.
    /// </summary>
    [FromQuery(Name = "photo_url")]
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Хэш всех свойств.
    /// </summary>
    [FromQuery(Name = "hash")]
    public string? Hash { get; set; } = null!;
}
