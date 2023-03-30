using System;
using Mkh.Data.Abstractions.Annotations;

namespace Mkh.Data.Abstractions.Entities;

/// <summary>
/// 实体软删除扩展
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// 已删除的
    /// </summary>
    [IgnoreOnEntityEvent]
    bool Deleted { get; set; }

    /// <summary>
    /// 删除人账户编号
    /// </summary>
    [IgnoreOnEntityEvent]
    Guid? DeletedBy { get; set; }

    /// <summary>
    /// 删除人名称
    /// </summary>
    [IgnoreOnEntityEvent]
    string Deleter { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [IgnoreOnEntityEvent]
    DateTime? DeletedTime { get; set; }
}