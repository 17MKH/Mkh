using System;

namespace Mkh.Domain.Abstractions.Entities;

/// <summary>
/// 实体
/// </summary>
public abstract class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; }

    protected Entity()
    {
    }

    protected Entity(TKey id)
    {
        Id = id;
    }
}

/// <summary>
/// 实体
/// </summary>
public class Entity : Entity<Guid>
{
    public Entity() : base(Guid.NewGuid())
    {
    }

    public Entity(Guid id) : base(id)
    {
    }
}