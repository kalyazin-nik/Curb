using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Curb.API.Contracts.Dtos;
using Curb.API.Contracts.Interfaces;
using Curb.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Curb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration configuration, IUserService userRepository) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IUserService _userRepository = userRepository;

    [HttpGet("telegram-callback")]
    public async Task<IActionResult> TelegramCallback([FromQuery] UserRegisterDto userRegister, CancellationToken cancellationToken)
    {
        if (!await VerifyTelegramData(userRegister, cancellationToken))
        {
            return Unauthorized("Ошибка проверки данных.");
        }

        var user = await _userRepository.GetByUserIdAsync(long.Parse(userRegister.Id), cancellationToken);
        if (user is not null)
        {
            await _userRepository.UpdateAsync(user, userRegister.MapToPropertyValues(), cancellationToken);
        }
        user ??= await _userRepository.RegisterAsync(userRegister, cancellationToken);
        var token = await GenerateToken(user, cancellationToken);
        return Ok(new { Token = token });
    }

    private async Task<bool> VerifyTelegramData(UserRegisterDto userRegister, CancellationToken cancellationToken)
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

    private async Task<string> GenerateToken(UserDto user, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username ?? ""),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"]!,
                audience: _configuration["Jwt:Audience"]!,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }, cancellationToken);
    }
}