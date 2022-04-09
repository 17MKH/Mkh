using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 使用PostgreSQL数据库
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="connectionString">连接字符串</param>
    /// <param name="configure">自定义配置</param>
    /// <returns></returns>
    public static IDbBuilder UsePostgreSQL(this IDbBuilder builder, string connectionString, Action<DbOptions> configure = null)
    {
        builder.UseDb(connectionString, DbProvider.PostgreSQL, configure);

        return builder;
    }
}