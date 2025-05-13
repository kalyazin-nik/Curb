namespace Curb.API.Contracts.Dtos;

/// <summary>
/// Вход пользователя в систему.
/// </summary>
public class UserLoginDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Электронная почта.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// Дата аутентификации.
    /// </summary>
    public string? AuthDate { get; set; } = null!;
}
