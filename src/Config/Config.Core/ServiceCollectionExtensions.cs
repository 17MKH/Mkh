using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mkh.Config.Abstractions;
using Mkh.Module.Abstractions;

namespace Mkh.Config.Core;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加配置功能
    /// </summary>
    /// <returns></returns>
    public static void AddConfig(this IServiceCollection services, IConfiguration cfg, IModuleCollection modules)
    {
        if (services.Any(m => m.ServiceType == typeof(IConfigProvider)))
        {
            return;
        }

        var configProvider = new DefaultConfigProvider(cfg);

        configProvider.AddModuleConfig(modules);

        services.TryAddSingleton<IConfigProvider>(configProvider);
    }
}