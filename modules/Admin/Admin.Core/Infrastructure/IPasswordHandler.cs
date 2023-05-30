namespace Mkh.Mod.Admin.Core.Infrastructure;

/// <summary>
/// 密码处理器接口
/// </summary>
public interface IPasswordHandler
{
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="password">明文密码</param>
    /// <returns></returns>
    string Encrypt(string password);

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="encryptPassword">密文密码</param>
    /// <returns></returns>
    string Decrypt(string encryptPassword);
}