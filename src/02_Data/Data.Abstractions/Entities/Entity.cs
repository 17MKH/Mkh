namespace Mkh.Data.Abstractions.Entities;

/// <summary>
/// 包含指定类型主键的实体
/// </summary>
public abstract class Entity<TKey> : IEntity
{
    /// <summary>
    /// 主键
    /// </summary>
    public virtual TKey Id { get; set; }
}

/// <summary>
/// 含有自增主键的实体
/// </summary>
public class Entity : Entity<int>
{

}