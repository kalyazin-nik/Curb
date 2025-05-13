using Curb.API.Contracts.Dtos;
using Curb.API.Contracts.Enums;
using Curb.API.DataAccess.Domains;

namespace Curb.API.Extensions;

internal static class MapUserExtensions
{
    public static User MapToUser(this UserRegisterDto userRegister)
    {
        return new User
        {
            UserId = userRegister.Id is not null ? long.Parse(userRegister.Id) : 0,
            Username = userRegister.Username,
            FirstName = userRegister.FirstName,
            LastName = userRegister.LastName,
            Email = userRegister.Email,
            Password = userRegister.Password,
            PhotoUrl = userRegister.PhotoUrl,
            AuthDate = userRegister.AuthDate is not null ? DateTimeOffset.FromUnixTimeSeconds(long.Parse(userRegister.AuthDate)).UtcDateTime : DateTime.UtcNow,
            CreeatedAt = DateTime.UtcNow,
            Session = new Session()
        };
    }

    public static UserDto MapToUserDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            RefreshToken = user.Session!.RefreshToken,
            Role = UserRole.Companion.ToString()
        };
    }

    public static UserDto MapToUserDto(this UserLoginDto userLogin)
    {
        return new UserDto
        {
            Id = userLogin.Id,
            Role = UserRole.Companion.ToString()
        };
    }

    public static Dictionary<string, object?> MapToPropertyValues(this UserRegisterDto userRegister, string refreshToken, int daysCount, bool forceLogout = false)
    {
        return new Dictionary<string, object?>
        {
            { nameof(userRegister.Username), userRegister.Username },
            { nameof(userRegister.FirstName), userRegister.FirstName },
            { nameof(userRegister.LastName), userRegister.LastName },
            { nameof(userRegister.PhotoUrl), userRegister.PhotoUrl },
            { nameof(userRegister.AuthDate),  userRegister.AuthDate is not null ? DateTimeOffset.FromUnixTimeSeconds(long.Parse(userRegister.AuthDate)).UtcDateTime : DateTime.UtcNow },
            { nameof(Session.RefreshToken),  refreshToken },
            { nameof(Session.ExpiryTime), DateTime.UtcNow.AddDays(daysCount) },
            { nameof(Session.ForceLogout), forceLogout },
        };
    }

    public static Dictionary<string, object?> MapToPropertyValues(this UserLoginDto userLogin, string refreshToken, int daysCount, bool forceLogout = false)
    {
        return new Dictionary<string, object?>
        {
            { nameof(userLogin.AuthDate),  userLogin.AuthDate is not null ? DateTimeOffset.FromUnixTimeSeconds(long.Parse(userLogin.AuthDate)).UtcDateTime : DateTime.UtcNow },
            { nameof(Session.RefreshToken),  refreshToken },
            { nameof(Session.ExpiryTime), DateTime.UtcNow.AddDays(daysCount) },
            { nameof(Session.ForceLogout), forceLogout },
        };
    }

    public static UserLoginDto ToUserLoginDto(this User user)
    {
        return new UserLoginDto
        {
            Id = user.Id,
            Email = user.Email,
            Password = user.Password
        };
    }
}
