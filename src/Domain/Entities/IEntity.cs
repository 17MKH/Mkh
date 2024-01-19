namespace Mkh.Domain.Entities;


/// <summary>
/// 实体
/// </summary>
public interface IEntity
{

}

/// <summary>
/// 实体
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntity<out TKey> : IEntity
{
    /// <summary>
    /// 主键
    /// </summary>
    public TKey Id { get; }
}