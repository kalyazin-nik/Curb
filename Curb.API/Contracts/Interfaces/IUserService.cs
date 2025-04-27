using Curb.API.Contracts.Dtos;

namespace Curb.API.Contracts.Interfaces;

public interface IUserService
{
    Task<UserDto> RegisterAsync(UserRegisterDto userRegister, CancellationToken cancellationToken);

    Task<UserDto?> GetByUserIdAsync(long userId, CancellationToken cancellationToken);

    Task UpdateAsync(UserDto userDto, Dictionary<string, object?> propertyValues, CancellationToken cancellationToken);
}
