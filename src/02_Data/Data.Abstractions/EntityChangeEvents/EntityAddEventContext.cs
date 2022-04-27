using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Data.Abstractions.EntityChangeEvents;

/// <summary>
/// 实体新增事件上下文
/// </summary>
public class EntityAddEventContext
{
    /// <summary>
    /// 实体描述符
    /// </summary>
    public IEntityDescriptor EntityDescriptor { get; set; }

    /// <summary>
    /// 新增实体
    /// </summary>
    public IEntity Entity { get; set; }
}