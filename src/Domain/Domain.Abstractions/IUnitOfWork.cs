using System;
using System.Threading.Tasks;

namespace Mkh.Domain.Abstractions;

/// <summary>
/// 工作单元接口
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// 保存变更
    /// </summary>
    Task CompleteAsync();

    /// <summary>
    /// 回滚
    /// </summary>
    Task RollbackAsync();
}