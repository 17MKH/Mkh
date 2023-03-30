using System;
using Mkh.Data.Abstractions.Annotations;

namespace Mkh.Data.Abstractions.Entities;

/// <summary>
/// 软删除基类实体
/// </summary>
public class EntityBaseSoftDelete<TKey> : EntityBase<TKey>, ISoftDelete
{
    /// <summary>
    /// 已删除的
    /// </summary>
    [IgnoreOnEntityEvent]
    public virtual bool Deleted { get; set; }

    /// <summary>
    /// 删除人编号
    /// </summary>
    [IgnoreOnEntityEvent]
    public virtual Guid? DeletedBy { get; set; }

    /// <summary>
    /// 删除人名称
    /// </summary>
    [Nullable]
    [IgnoreOnEntityEvent]
    public virtual string Deleter { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [IgnoreOnEntityEvent]
    public virtual DateTime? DeletedTime { get; set; }
}

/// <summary>
/// 软删除基类实体
/// </summary>
public class EntityBaseSoftDelete : EntityBaseSoftDelete<int>
{

}