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

        var configProvider = new ConfigProvider();

        services.AddCommonConfig(cfg, configProvider);

        services.AddModuleConfig(cfg, modules, configProvider);

        services.TryAddSingleton<IConfigProvider>(configProvider);
    }

    /// <summary>
    /// 添加通用配置
    /// </summary>
    /// <param name="services"></param>
    /// <param name="cfg"></param>
    /// <param name="configProvider"></param>
    private static void AddCommonConfig(this IServiceCollection services, IConfiguration cfg, ConfigProvider configProvider)
    {
        //添加通用配置
        var commonConfig = new CommonConfig();
        cfg.GetSection("Mkh:Common").Bind(commonConfig);
        if (commonConfig.TempDir.IsNull())
        {
            commonConfig.TempDir = Path.Combine(AppContext.BaseDirectory, "Temp");
        }

        configProvider.Add(commonConfig);
    }

    /// <summary>
    /// 添加模块配置
    /// </summary>
    /// <param name="services"></param>
    /// <param name="cfg"></param>
    /// <param name="modules"></param>
    /// <param name="configProvider"></param>
    private static void AddModuleConfig(this IServiceCollection services, IConfiguration cfg, IModuleCollection modules, ConfigProvider configProvider)
    {
        foreach (var module in modules)
        {
            var configType = module.LayerAssemblies.Core.GetTypes().FirstOrDefault(m => typeof(IConfig).IsImplementType(m));
            if (configType != null)
            {
                var instance = (IConfig)Activator.CreateInstance(configType)!;
                cfg.GetSection($"Mkh:Modules:{module.Code}:Config").Bind(instance);
                configProvider.Add(instance);
            }
        }
    }
}