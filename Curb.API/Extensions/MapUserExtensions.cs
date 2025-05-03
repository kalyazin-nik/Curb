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
            UserId = long.Parse(userRegister.Id),
            Username = userRegister.Username,
            FirstName = userRegister.FirstName,
            LastName = userRegister.LastName,
            PhotoUrl = userRegister.PhotoUrl,
            AuthDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(userRegister.AuthDate)).UtcDateTime,
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

    public static Dictionary<string, object?> MapToPropertyValues(this UserRegisterDto userRegister)
    {
        return new Dictionary<string, object?>
        {
            { nameof(userRegister.Username), userRegister.Username },
            { nameof(userRegister.FirstName), userRegister.FirstName },
            { nameof(userRegister.LastName), userRegister.LastName },
            { nameof(userRegister.PhotoUrl), userRegister.PhotoUrl },
            { nameof(userRegister.AuthDate),  DateTimeOffset.FromUnixTimeSeconds(long.Parse(userRegister.AuthDate)).UtcDateTime }
        };
    }
}
