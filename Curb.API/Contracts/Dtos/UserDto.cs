namespace Curb.API.Contracts.Dtos;

/// <summary>
/// Пользователь.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Прозвище.
    /// </summary>
    public string? Username { get; set; } = null!;

    /// <summary>
    /// Токен обновления.
    /// </summary>
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// Роль.
    /// </summary>
    public string Role { get; set; } = null!;
}
