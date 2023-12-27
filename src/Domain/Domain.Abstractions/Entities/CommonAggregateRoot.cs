using System;

namespace Mkh.Domain.Abstractions.Entities;

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

    protected CommonAggregateRoot()
    {
    }

    protected CommonAggregateRoot(TKey id) : base(id)
    {
    }

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

public abstract class CommonAggregateRoot : CommonAggregateRoot<Guid>
{
    protected CommonAggregateRoot() : base(Guid.NewGuid())
    {
    }

    protected CommonAggregateRoot(Guid createdBy) : base(Guid.NewGuid(), createdBy)
    {
    }
}