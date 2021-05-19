using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mkh;
using Mkh.Data;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Logger;
using Mkh.Data.Abstractions.Options;
using Mkh.Data.Core;
using Mkh.Data.Core.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Mkh数据库核心
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure">自定义配置项</param>
        /// <returns></returns>
        public static IDbBuilder AddMkhDb<TDbContext>(this IServiceCollection services, Action<DbOptions> configure = null)
            where TDbContext : IDbContext
        {
            var options = new DbOptions();

            configure?.Invoke(options);

            Check.NotNull(options.ConnectionString, "连接字符串未配置");

            //尝试添加默认账户信息解析器
            services.TryAddSingleton<IAccountResolver, DefaultAccountResolver>();
            //尝试添加默认的数据库操作日志记录器
            services.TryAddSingleton<IDbLoggerProvider, DbLoggerProvider>();

            return new DbBuilder(services, options, typeof(TDbContext));
        }
    }
}
