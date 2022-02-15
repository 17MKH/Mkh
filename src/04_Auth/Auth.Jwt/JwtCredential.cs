using System;
using Mkh.Auth.Abstractions;

namespace Mkh.Auth.Jwt;

/// <summary>
/// JWT返回模型
/// </summary>
[Serializable]
public class JwtCredential : ICredential
{
    /// <summary>
    /// 账户编号
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// 访问令牌
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// 刷新令牌
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// 访问令牌有效期
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// 登录时间戳
    /// </summary>
    public long LoginTime { get; set; }
}