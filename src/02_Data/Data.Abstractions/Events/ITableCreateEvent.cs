using System;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Abstractions.Events;

/// <summary>
/// 表创建事件
/// </summary>
public interface ITableCreateEvent
{
    /// <summary>
    /// 创建前的事件
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnBeforeCreate(TableCreateContext context);

    /// <summary>
    /// 创建后的事件
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnAfterCreate(TableCreateContext context);
}

/// <summary>
/// 表创建上下文
/// </summary>
public class TableCreateContext
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
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}