using System;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Options;

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
        /// <returns></returns>
        public static IDbBuilder AddCodeFirst(this IDbBuilder builder)
        {
            return builder.AddCodeFirst(null);
        }

        /// <summary>
        /// 添加CodeFirst功能
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure">自定义配置</param>
        /// <returns></returns>
        public static IDbBuilder AddCodeFirst(this IDbBuilder builder, Action<CodeFirstOptions> configure)
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

                    //在有表
                    provider.CreateTable();
                }
            });

            return builder;
        }
    }
}
