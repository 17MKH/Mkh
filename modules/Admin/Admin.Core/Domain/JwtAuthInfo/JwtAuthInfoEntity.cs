using System;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.JwtAuthInfo;

/// <summary>
/// JWT认证信息
/// </summary>
public class JwtAuthInfoEntity : Entity
{
    /// <summary>
    /// 账户编号
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// 登录平台
    /// </summary>
    public int Platform { get; set; }

    /// <summary>
    /// 刷新令牌
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// 刷新令牌过期时间
    /// </summary>
    public DateTime RefreshTokenExpiredTime { get; set; }

    /// <summary>
    /// 最后登录时间戳
    /// </summary>
    public long LoginTime { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    [Nullable]
    public string LoginIP { get; set; }
}