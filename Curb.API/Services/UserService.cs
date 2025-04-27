using Curb.API.Contracts.Dtos;
using Curb.API.Contracts.Interfaces;
using Curb.API.DataAccess;
using Curb.API.DataAccess.Domains;
using Curb.API.Extensions;

namespace Curb.API.Services;

public class UserService(IRepository<User, CurbDbContext> repository) : IUserService
{
    private readonly IRepository<User, CurbDbContext> _repository = repository;

    public async Task<UserDto> RegisterAsync(UserRegisterDto userRegister, CancellationToken cancellationToken)
    {
        var user = userRegister.MapToUser();
        await _repository.AddAsync(user, cancellationToken);
        return user.MapToUserDto();
    }

    public async Task<UserDto?> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
    {
        return await Task.Run(() => _repository.GetByPredicate(x => x.UserId == userId).SingleOrDefault()?.MapToUserDto(), cancellationToken);
    }

    public async Task UpdateAsync(UserDto userDto, Dictionary<string, object?> propertyValues, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(userDto.Id, cancellationToken);
        var userProperties = typeof(User).GetProperties().ToDictionary(x => x.Name, v => v);
        foreach (var propertyValue in propertyValues)
        {
            //var value = userProperties[propertyValue.Key].PropertyType switch
            //{
            //    { } t when t == typeof(string) => propertyValue.Value,
            //    { } t when t == typeof(DateTime) => DateTime.Parse(propertyValue.Value!.ToString()!),
            //    _ => throw new ArgumentException($"Неизвестный тип данных для свойства {propertyValue.Key}")
            //};
            userProperties[propertyValue.Key].SetValue(user, propertyValue.Value);
        }

        await _repository.UpdateAsync(user, cancellationToken);
    }
}
