using Curb.API.Contracts.Dtos;
using Curb.API.Contracts.Enums;
using Curb.API.DataAccess.Domains;

namespace Curb.API.Extensions;

public static class MapUserExtensions
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
            AuthDate = DateTime.Parse(userRegister.AuthDate!),
            CreeatedAt = DateTime.UtcNow
        };
    }

    public static UserDto MapToUserDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = UserRole.Companion
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
