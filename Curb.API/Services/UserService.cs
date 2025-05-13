using System.Reflection;
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
/// <param name="securityService"></param>
internal class UserService(IRepository<CurbDbContext> repository, ISecurityService securityService) : IUserService
{
    private readonly IRepository<CurbDbContext> _repository = repository;
    private readonly ISecurityService _securityService = securityService;
    private static readonly Dictionary<string, PropertyInfo> _userProperties = typeof(User).GetProperties().ToDictionary(x => x.Name);
    private static readonly Dictionary<string, PropertyInfo> _sessionProperties = typeof(Session).GetProperties().ToDictionary(x => x.Name);

    /// <inheritdoc />
    public async Task<UserDto> RegisterAsync(UserRegisterDto userRegister, CancellationToken cancellationToken)
    {
        var user = userRegister.MapToUser();
        if (user.Password is not null)
        {
            user.Password = _securityService.GetPasswordHash(user.Password);
        }
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

    public async Task<UserDto?> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByPredicate<User>(x => x.Email == email).SingleOrDefaultAsync(cancellationToken);
        if (user is not null)
        {
            user.Session ??= await _repository.GetByPredicate<Session>(x => x.UserId == user.Id).SingleOrDefaultAsync(cancellationToken);
        }

        return user?.MapToUserDto();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Guid id, Dictionary<string, object?> propertyValues, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync<User>(id, cancellationToken);
        user.Session ??= await _repository.GetByPredicate<Session>(x => x.UserId == user.Id).SingleAsync(cancellationToken);
        foreach (var propertyValue in propertyValues)
        {
            if (_userProperties.TryGetValue(propertyValue.Key, out var userProperty))
            {
                userProperty.SetValue(user, propertyValue.Value);
            }
            else if (_sessionProperties.TryGetValue(propertyValue.Key, out var sessionProperty))
            {
                sessionProperty.SetValue(user.Session, propertyValue.Value);
            }
        }

        await _repository.UpdateAsync(user, cancellationToken);
    }

    public async Task<UserLoginDto?> GetUserLogin(string email, CancellationToken cancellationToken)
    {
        return (await _repository.GetByPredicate<User>(x => x.Email == email).SingleOrDefaultAsync(cancellationToken))?.ToUserLoginDto();
    }
}
