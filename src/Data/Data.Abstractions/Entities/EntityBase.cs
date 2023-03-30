using System;
using Mkh.Data.Abstractions.Annotations;

namespace Mkh.Data.Abstractions.Entities;

/// <summary>
/// 通用实体基类
/// </summary>
/// <typeparam name="TKey"></typeparam>
public class EntityBase<TKey> : Entity<TKey>
{
    /// <summary>
    /// 创建人编号
    /// </summary>
    [IgnoreOnEntityEvent]
    public virtual Guid CreatedBy { get; set; }

    /// <summary>
    /// 创建人名称
    /// </summary>
    [IgnoreOnEntityEvent]
    public virtual string Creator { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [IgnoreOnEntityEvent]
    public virtual DateTime CreatedTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 修改人编号
    /// </summary>
    [IgnoreOnEntityEvent]
    public virtual Guid? ModifiedBy { get; set; }

    /// <summary>
    /// 修改人名称
    /// </summary>
    [Nullable]
    [IgnoreOnEntityEvent]
    public virtual string Modifier { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [IgnoreOnEntityEvent]
    public virtual DateTime? ModifiedTime { get; set; }
}

/// <summary>
/// 通用实体基类，主键类型采用自增Int
/// </summary>
public class EntityBase : EntityBase<int>
{

}