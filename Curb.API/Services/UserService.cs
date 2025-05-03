using Curb.API.Contracts.Dtos;
using Curb.API.Contracts.Interfaces;
using Curb.API.DataAccess;
using Curb.API.DataAccess.Domains;
using Curb.API.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Curb.API.Services;

/// <summary>
/// Сервис по работе с пользователями.
/// </summary>
/// <param name="repository">Репозиторий.</param>
internal class UserService(IRepository<CurbDbContext> repository) : IUserService
{
    private readonly IRepository<CurbDbContext> _repository = repository;

    /// <inheritdoc />
    public async Task<UserDto> RegisterAsync(UserRegisterDto userRegister, CancellationToken cancellationToken)
    {
        var user = userRegister.MapToUser();
        await _repository.AddAsync(user, cancellationToken);
        return user.MapToUserDto();
    }

    /// <inheritdoc />
    public async Task<UserDto?> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByPredicate<User>(x => x.UserId == userId).SingleOrDefaultAsync(cancellationToken);
        if (user is not null)
        {
            user.Session ??= await _repository.GetByPredicate<Session>(x => x.UserId == user.Id).SingleOrDefaultAsync(cancellationToken);
        }

        return user?.MapToUserDto();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(UserDto userDto, Dictionary<string, object?> propertyValues, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync<User>(userDto.Id, cancellationToken);
        var userProperties = typeof(User).GetProperties().ToDictionary(x => x.Name, v => v);
        foreach (var propertyValue in propertyValues)
        {
            userProperties[propertyValue.Key].SetValue(user, propertyValue.Value);
        }

        user.Session ??= await _repository.GetByPredicate<Session>(x => x.UserId == user.Id).SingleAsync(cancellationToken);
        user.Session!.RefreshToken = userDto.RefreshToken;
        user.Session.ExpiryTime = DateTime.UtcNow.AddDays(2);

        await _repository.UpdateAsync(user, cancellationToken);
    }
}
