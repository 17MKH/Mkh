using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions.Options;

namespace Mkh.Data.Abstractions;

public interface IDbBuilder
{
    /// <summary>
    /// 服务集合
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// 数据库配置项
    /// </summary>
    DbOptions Options { get; }

    /// <summary>
    /// 代码优先配置项
    /// </summary>
    CodeFirstOptions CodeFirstOptions { get; set; }

    /// <summary>
    /// 数据库上下文
    /// </summary>
    IDbContext DbContext { get; }

    /// <summary>
    /// 从指定程序集加载仓储
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    IDbBuilder AddRepositoriesFromAssembly(Assembly assembly);

    /// <summary>
    /// 添加自定义委托方法，可用于扩展功能
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    IDbBuilder AddAction(Action action);

    /// <summary>
    /// 构建
    /// </summary>
    void Build();
}