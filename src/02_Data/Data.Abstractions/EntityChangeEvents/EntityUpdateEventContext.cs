using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Data.Abstractions.EntityChangeEvents;

/// <summary>
/// 实体更新事件上下文
/// </summary>
public class EntityUpdateEventContext
{
    /// <summary>
    /// 实体描述符
    /// </summary>
    public IEntityDescriptor EntityDescriptor { get; set; }

    /// <summary>
    /// 变更实体
    /// </summary>
    public IEntity Entity { get; set; }
}