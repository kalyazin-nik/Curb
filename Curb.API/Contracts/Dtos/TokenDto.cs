namespace Curb.API.Contracts.Dtos;

/// <summary>
/// Токен.
/// </summary>
public class TokenDto
{
    /// <summary>
    /// Токен доступа.
    /// </summary>
    public string AccessToken { get; set; } = null!;

    /// <summary>
    /// Токен обновления.
    /// </summary>
    public string RefreshToken { get; set; } = null!;
}
