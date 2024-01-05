using System;

namespace Mkh.Domain.Abstractions.Entities;

/// <summary>
/// 通用聚合根，包含创建人编号、创建时间、最后修改人编号、最后修改时间
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class CommonAggregateRoot<TKey> : AggregateRoot<TKey>
{
    /// <summary>
    /// 创建人编号
    /// </summary>
    public Guid CreatedBy { get; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTimeOffset CreatedTime { get; }

    /// <summary>
    /// 最后修改人编号
    /// </summary>
    public Guid? LastModifiedBy { get; private set; }

    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTimeOffset? LastModifiedTime { get; private set; }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="id">主键</param>
    protected CommonAggregateRoot(TKey id) : base(id)
    {
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="createdBy">创建人</param>
    protected CommonAggregateRoot(TKey id, Guid createdBy) : base(id)
    {
        CreatedBy = createdBy;
        CreatedTime = DateTimeOffset.Now;
    }

    /// <summary>
    /// 设置修改人
    /// </summary>
    /// <param name="modifiedBy"></param>
    public void SetModifiedBy(Guid modifiedBy)
    {
        LastModifiedBy = modifiedBy;
        LastModifiedTime = DateTimeOffset.Now;
    }
}

/// <summary>
/// 主键类型为Guid的通用聚合根
/// </summary>
public abstract class CommonAggregateRoot : CommonAggregateRoot<Guid>
{
    protected CommonAggregateRoot() : base(Guid.NewGuid())
    {
    }

    protected CommonAggregateRoot(Guid createdBy) : base(Guid.NewGuid(), createdBy)
    {
    }
}