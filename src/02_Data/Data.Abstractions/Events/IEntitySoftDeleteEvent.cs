using System;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Abstractions.Events;

/// <summary>
/// 实体软删除事件
/// </summary>
public interface IEntitySoftDeleteEvent
{
    /// <summary>
    /// 软删除事件
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnSoftDelete(EntitySoftDeleteContext context);
}

/// <summary>
/// 实体新增事件上下文
/// </summary>
public class EntitySoftDeleteContext
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public IDbContext DbContext { get; set; }

    /// <summary>
    /// 实体描述符
    /// </summary>
    public IEntityDescriptor EntityDescriptor { get; set; }

    /// <summary>
    /// 实体编号
    /// </summary>
    public object Id { get; set; }

    /// <summary>
    /// 自定义表名称，用于分表情况
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// 工作单元
    /// </summary>
    public IUnitOfWork Uow { get; set; }

    /// <summary>
    /// 租户编号
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 操作人编号
    /// </summary>
    public Guid? Operator { get; set; }

    /// <summary>
    /// 操作人名称
    /// </summary>
    public string OperatorName { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime DeleteTime { get; set; }
}
