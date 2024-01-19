using System;

namespace Mkh.Domain.Entities;

/// <summary>
/// 实体
/// </summary>
public abstract class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; }

    protected Entity(TKey id)
    {
        Id = id;
    }
}

/// <summary>
/// 实体
/// </summary>
public abstract class Entity : Entity<Guid>
{
    protected Entity() : base(Guid.NewGuid())
    {
    }

    protected Entity(Guid id) : base(id)
    {
    }
}