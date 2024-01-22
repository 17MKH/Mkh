using System;

namespace Mkh.Identity.Abstractions;

/// <summary>
/// 当前租户
/// </summary>
public interface ICurrentTenant
{
    /// <summary>
    /// 租户编号
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// 租户名称
    /// </summary>
    string Name { get; }
}