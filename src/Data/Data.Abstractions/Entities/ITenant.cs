using System;

namespace Mkh.Data.Abstractions.Entities;

/// <summary>
/// 实体租户扩展
/// </summary>
public interface ITenant<TKey>
{
    /// <summary>
    /// 租户编号
    /// </summary>
    TKey TenantId { get; set; }
}

/// <summary>
/// 实体租户扩展
/// </summary>
public interface ITenant : ITenant<Guid?>
{

}