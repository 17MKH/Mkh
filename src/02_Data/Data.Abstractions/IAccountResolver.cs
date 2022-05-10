using System;

namespace Mkh.Data.Abstractions;

/// <summary>
/// 操作员信息解析器
/// </summary>
public interface IOperatorResolver
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
    public string AccountName { get; }
}