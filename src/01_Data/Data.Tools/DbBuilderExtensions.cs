using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mkh.Data.Abstractions;
using Mkh.Data.Tools.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 数据库生成器扩展
    /// </summary>
    public static class DbBuilderExtensions
    {
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

            //先添加服务的实现类，以便使用它的实例来创建代理实例
            services.AddScoped(implementationType);

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
