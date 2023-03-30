using System;
using System.Data;

namespace Mkh.Data.Abstractions;

/// <summary>
/// 工作单元接口
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// 事务
    /// </summary>
    IDbTransaction Transaction { get; }

    /// <summary>
    /// 保存变更
    /// </summary>
    void SaveChanges();

    /// <summary>
    /// 回滚
    /// </summary>
    void Rollback();
}