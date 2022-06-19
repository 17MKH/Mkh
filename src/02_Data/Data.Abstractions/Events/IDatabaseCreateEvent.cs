using System;
using System.Threading.Tasks;

namespace Mkh.Data.Abstractions.Events;

/// <summary>
/// 数据库创建事件
/// </summary>
public interface IDatabaseCreateEvent
{
    /// <summary>
    /// 创建前的事件
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnBeforeCreate(DatabaseCreateContext context);

    /// <summary>
    /// 创建后的事件
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnAfterCreate(DatabaseCreateContext context);
}

/// <summary>
/// 数据库创建上下文
/// </summary>
public class DatabaseCreateContext
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public IDbContext DbContext { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}