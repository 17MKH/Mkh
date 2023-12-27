using System;

namespace Mkh.Domain.Abstractions.Entities;

/// <summary>
/// 聚合根
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IAggregateRoot<TKey> : IEntity<TKey>
{
}

/// <summary>
/// 聚合根
/// </summary>
public interface IAggregateRoot : IAggregateRoot<Guid>
{

}