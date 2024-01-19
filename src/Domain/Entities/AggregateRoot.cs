using System;

namespace Mkh.Domain.Entities;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
{
    protected AggregateRoot(TKey id) : base(id)
    {
    }
}

public abstract class AggregateRoot : Entity<Guid>, IAggregateRoot<Guid>
{
    protected AggregateRoot(Guid id) : base(id)
    {
    }

    protected AggregateRoot() : base(Guid.NewGuid())
    {

    }
}