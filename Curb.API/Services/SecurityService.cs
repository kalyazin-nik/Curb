using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Curb.API.Contracts.Dtos;
using Curb.API.Contracts.Interfaces;
using Curb.API.Extensions;

namespace Curb.API.Services;

/// <summary>
/// 
/// </summary>
internal class SecurityService : ISecurityService
{
    /// <inheritdoc />
    public string GetPasswordHash(string password)
    {
        return GetPasswordHash(password, GetSalt());
    }

    /// <inheritdoc />
    public bool IsPasswordVerified(string password, string hashPassword)
    {
        return hashPassword == GetPasswordHash(password, GetSalt());
    }

    private static string GetPasswordHash(string password, string salt)
    {
        var iterations = 10000;
        var saltBytes = Convert.FromBase64String(salt);
        using var rfc2898 = new Rfc2898DeriveBytes(password, saltBytes, iterations, HashAlgorithmName.SHA256);
        var hashBytes = rfc2898.GetBytes(32);
        return Convert.ToBase64String(hashBytes);
    }

    private static string GetSalt()
    {
        var guid = GenerateGuid(Assembly.GetExecutingAssembly().GetName().Name!);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(guid.ToString()));
    }

    private static Guid GenerateGuid(string input)
    {
        var guidBytes = new byte[16];
        var hashBytes = SHA1.HashData(Encoding.UTF8.GetBytes(input));
        Array.Copy(hashBytes, guidBytes, guidBytes.Length);
        return new Guid(guidBytes);
    }

    /// <inheritdoc />
    public bool IsTelegramDataVerified(UserRegisterDto userRegister, string secret)
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
        var secretKey = SHA256.HashData(Encoding.UTF8.GetBytes(secret));
        using var hmac = new HMACSHA256(secretKey);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()));
        var computedHashHex = BitConverter.ToString(computedHash).Replace("-", "").ToLower();

        return computedHashHex == userRegister.Hash;
    }
}
