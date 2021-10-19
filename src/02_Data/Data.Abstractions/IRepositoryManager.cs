using System.Collections.Generic;

namespace Mkh.Data.Abstractions;

/// <summary>
/// 仓储管理器，用于管理当前请求中(Scoped模式注入)的仓储实例
/// </summary>
public interface IRepositoryManager
{
    /// <summary>
    /// 仓储实例列表
    /// </summary>
    List<IRepository> Repositories { get; }

    /// <summary>
    /// 添加仓储实例
    /// </summary>
    /// <param name="repository"></param>
    void Add(IRepository repository);
}