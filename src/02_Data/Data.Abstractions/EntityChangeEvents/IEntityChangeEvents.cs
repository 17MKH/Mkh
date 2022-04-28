using System.Threading.Tasks;

namespace Mkh.Data.Abstractions.EntityChangeEvents;

/// <summary>
/// 实体变更事件
/// </summary>
public interface IEntityChangeEvents
{
    /// <summary>
    /// 新增事件
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnAdd(EntityAddEventContext context);

    /// <summary>
    /// 更新事件
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnUpdate(EntityUpdateEventContext context);

    /// <summary>
    /// 删除事件
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnDelete(EntityDeleteEventContext context);

    /// <summary>
    /// 软删除事件
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnSoftDelete(EntitySoftDeleteEventContext context);
}