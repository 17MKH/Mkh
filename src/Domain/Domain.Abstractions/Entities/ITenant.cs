using System;

namespace Mkh.Domain.Abstractions.Entities;

/// <summary>
/// 多租户
/// </summary>
public interface ITenant
{
    /// <summary>
    /// 租户编号
    /// </summary>
    Guid? TenantId { get; }
}