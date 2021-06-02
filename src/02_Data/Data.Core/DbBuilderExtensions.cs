using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Options;
using Mkh.Data.Core.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class DbBuilderExtensions
    {
        /// <summary>
        /// 使用数据库
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString"></param>
        /// <param name="provider"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IDbBuilder UseDb(this IDbBuilder builder, string connectionString, DbProvider provider, Action<DbOptions> configure = null)
        {
            builder.Options.ConnectionString = connectionString;
            builder.Options.Provider = provider;
            configure?.Invoke(builder.Options);

            return builder;
        }

        /// <summary>
        /// 添加CodeFirst功能
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure">自定义配置</param>
        /// <returns></returns>
        public static IDbBuilder AddCodeFirst(this IDbBuilder builder, Action<CodeFirstOptions> configure = null)
        {
            var options = new CodeFirstOptions();
            configure?.Invoke(options);

            builder.CodeFirstOptions = options;

            builder.AddAction(() =>
            {
                //优先使用自定义的代码优先提供器，毕竟默认的建库见表语句并不能满足所有人的需求
                ICodeFirstProvider provider = options.CustomCodeFirstProvider;
                if (provider == null)
                {
                    provider = builder.DbContext.CodeFirstProvider;
                }

                if (provider != null)
                {
                    //先有库
                    if (options.CreateDatabase)
                    {
                        provider.CreateDatabase();
                    }

                    //后有表
                    provider.CreateTable();
                }
            });

            return builder;
        }

        /// <summary>
        /// 添加事务特性功能
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public static IDbBuilder AddTransactionAttribute(this IDbBuilder builder, Type serviceType, Type implementationType)
        {
            var services = builder.Services;

            //尝试添加代理生成器
            services.TryAddSingleton<IProxyGenerator, ProxyGenerator>();

            //添加需要特性事务的服务
            services.AddScoped(serviceType, sp =>
            {
                var target = sp.GetService(implementationType);
                var generator = sp.GetService<IProxyGenerator>();
                var manager = sp.GetService<IRepositoryManager>();
                var interceptor = new TransactionInterceptor(builder.DbContext, manager);
                var proxy = generator!.CreateInterfaceProxyWithTarget(serviceType, target, interceptor);
                return proxy;
            });

            return builder;
        }
    }
}
