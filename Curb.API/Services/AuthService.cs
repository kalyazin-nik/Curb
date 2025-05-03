using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Curb.API.Contracts.Dtos;
using Curb.API.Contracts.Interfaces;
using Curb.API.DataAccess;
using Curb.API.DataAccess.Domains;
using Curb.API.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Curb.API.Services;

/// <summary>
/// Сервис по работе аутентификацией и авторизацией.
/// </summary>
/// <param name="configuration">Конфигурация приложения.</param>
/// <param name="repository">Репозиторий.</param>
internal class AuthService(IConfiguration configuration, IRepository<CurbDbContext> repository) : IAuthService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IRepository<CurbDbContext> _repository = repository;

    /// <inheritdoc />
    public async Task<bool> VerifyTelegramData(UserRegisterDto userRegister, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            var builder = new StringBuilder();
            foreach (var property in typeof(UserRegisterDto).GetProperties().OrderBy(x => x.Name))
            {
                if (property.Name != nameof(UserRegisterDto.Hash) && property.GetValue(userRegister) is object value)
                {
                    builder.Append($"{property.Name.ToStringSnakeCase()}={value}\n");
                }
            }
            builder.Length--;
            var secretKey = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(_configuration["BotConfiguration:BotToken"]!));
            using var hmac = new HMACSHA256(secretKey);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()));
            var computedHashHex = BitConverter.ToString(computedHash).Replace("-", "").ToLower();

            return computedHashHex == userRegister.Hash;
        }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> GenerateAccessToken(UserDto user, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            var claims = new[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("role", user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"]!,
                audience: _configuration["Jwt:Audience"]!,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> GenerateRefreshToken(CancellationToken cancellationToken)
    {
        return await Task.Run(() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> RefreshTokenIsValidAsync(UserDto user, CancellationToken cancellationToken)
    {
        var session = await _repository.GetByPredicate<Session>(x => x.UserId == user.Id).SingleAsync(cancellationToken);
        return session.RefreshToken == user.RefreshToken && !session.ForceLogout && session.ExpiryTime > DateTime.UtcNow;
    }

    /// <inheritdoc />
    public async Task UserLogout(UserDto user, CancellationToken cancellationToken)
    {
        var session = await _repository.GetByPredicate<Session>(x => x.UserId == user.Id).SingleOrDefaultAsync(cancellationToken);
        
        if (session is not null)
        {
            session.ForceLogout = true;
            await _repository.UpdateAsync(session, cancellationToken);
        }
    }
}
