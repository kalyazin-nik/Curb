using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Curb.API.Contracts;
using Curb.API.Contracts.Dtos;
using Curb.API.Contracts.Interfaces;
using Curb.API.DataAccess;
using Curb.API.DataAccess.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Curb.API.Services;

/// <summary>
/// Сервис по работе аутентификацией и авторизацией.
/// </summary>
/// <param name="configuration">Конфигурация приложения.</param>
/// <param name="repository">Репозиторий.</param>
/// <param name="securityService"></param>
internal class AuthService(ApiConfiguration configuration, IRepository<CurbDbContext> repository, ISecurityService securityService) : IAuthService
{
    private readonly ApiConfiguration _configuration = configuration;
    private readonly IRepository<CurbDbContext> _repository = repository;
    private readonly ISecurityService _securityService = securityService;

    /// <inheritdoc />
    public async Task<bool> VerifyTelegramData(UserRegisterDto userRegister, CancellationToken cancellationToken)
    {
        return await Task.Run(() => _securityService.IsTelegramDataVerified(userRegister, _configuration.BotToken), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> GenerateAccessToken(UserDto user, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username ?? ""),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("userId", user.Id.ToString()),
                new Claim("role", user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.Issuer!,
                audience: _configuration.Audience,
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
    public async Task<bool> AccessTokenIsValidAsync(string accessToken, CancellationToken cancellationToken, bool validateIssuer = true, bool validateAudience = true, bool validateLifetime = false)
    {
        return await Task.Run(() => 
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = validateIssuer,
                ValidateAudience = validateAudience,
                ValidateLifetime = validateLifetime,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key)),
                ValidIssuer = _configuration.Issuer,
                ValidAudience = _configuration.Audience,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
            };

            new JwtSecurityTokenHandler().ValidateToken(accessToken, validationParameters, out var validatedToken);
            return validatedToken != null;
        }, cancellationToken);
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
