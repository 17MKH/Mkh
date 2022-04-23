using Microsoft.Extensions.DependencyInjection;
using Mkh.Logging.Abstractions.Providers;
using Mkh.Logging.Core.DefaultProviders;

namespace Mkh.Logging.Core;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加日志功能
    /// </summary>
    /// <param name="services"></param>
    public static void AddMkhLogging(this IServiceCollection services)
    {
        //登录日志处理器
        services.AddScoped<ILoginLogProvider, LoginLogProvider>();
    }
}