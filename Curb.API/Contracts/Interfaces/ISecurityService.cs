using Curb.API.Contracts.Dtos;

namespace Curb.API.Contracts.Interfaces;

/// <summary>
/// 
/// </summary>
public interface ISecurityService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    string GetPasswordHash(string password);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="password"></param>
    /// <param name="hashPassword"></param>
    /// <returns></returns>
    bool IsPasswordVerified(string password, string hashPassword);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userRegister"></param>
    /// <param name="secret"></param>
    /// <returns></returns>
    bool IsTelegramDataVerified(UserRegisterDto userRegister, string secret);
}
