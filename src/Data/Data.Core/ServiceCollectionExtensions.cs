using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Logger;
using Mkh.Data.Abstractions.Options;
using Mkh.Data.Core;
using Mkh.Data.Core.Internal;
using Mkh.Data.Core.Sharding;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加Mkh数据库核心
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure">自定义配置项委托</param>
    /// <returns></returns>
    public static IDbBuilder AddMkhDb<TDbContext>(this IServiceCollection services, Action<DbOptions> configure = null)
        where TDbContext : IDbContext
    {
        return services.AddMkhDb(typeof(TDbContext), configure);
    }

    /// <summary>
    /// 添加Mkh数据库核心功能
    /// </summary>
    /// <param name="services"></param>
    /// <param name="dbContextType">数据库上下文类型</param>
    /// <param name="configure">自定义配置项委托</param>
    /// <returns></returns>
    public static IDbBuilder AddMkhDb(this IServiceCollection services, Type dbContextType, Action<DbOptions> configure = null)
    {
        var options = new DbOptions();

        configure?.Invoke(options);

        //添加仓储实例管理器
        services.AddScoped<IRepositoryManager, RepositoryManager>();

        //尝试添加默认账户信息解析器
        services.TryAddSingleton<IOperatorResolver, DefaultOperatorResolver>();

        //尝试添加默认的数据库操作日志记录器
        services.TryAddSingleton<IDbLoggerProvider, DbLoggerProvider>();

        return new DbBuilder(services, options, dbContextType);
    }
}