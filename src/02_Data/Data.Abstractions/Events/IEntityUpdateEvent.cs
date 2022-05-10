using System;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Data.Abstractions.Events;

/// <summary>
/// 实体更新事件
/// </summary>
public interface IEntityUpdateEvent
{
    /// <summary>
    /// 事件
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnUpdate(EntityUpdateContext context);
}
/// <summary>
/// 实体新增事件上下文
/// </summary>
public class EntityUpdateContext
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
    /// 自定义表名称，用于分表情况
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// 原有实体
    /// </summary>
    public IEntity OldEntity { get; set; }

    /// <summary>
    /// 更新实体
    /// </summary>
    public IEntity NewEntity { get; set; }

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
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}
