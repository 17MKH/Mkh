using System;
using System.Threading.Tasks;

namespace Mkh.Logging.Abstractions.Providers;

/// <summary>
/// 登录日志提供器
/// </summary>
public interface ILoginLogProvider
{
    /// <summary>
    /// 写入日志
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> Write(LoginLogModel model);
}

/// <summary>
/// 登录日志模型
/// </summary>
public class LoginLogModel
{
    /// <summary>
    /// 账户编号
    /// </summary>
    public Guid? AccountId { get; set; }

    /// <summary>
    /// 平台
    /// </summary>
    public int Platform { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    public string VerifyCode { get; set; }

    /// <summary>
    /// 验证码ID
    /// </summary>
    public string VerifyCodeId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }
}