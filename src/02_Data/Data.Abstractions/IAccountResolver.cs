using System;

namespace Mkh.Data.Abstractions;

/// <summary>
/// 账户信息解析器
/// </summary>
public interface IAccountResolver
{
    /// <summary>
    /// 租户编号
    /// </summary>
    public Guid? TenantId { get; }

    /// <summary>
    /// 账户编号
    /// </summary>
    public Guid? AccountId { get; }

    /// <summary>
    /// 账户名称
    /// </summary>
    public string Username { get; }
}