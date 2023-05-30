using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Mkh.Config.Abstractions;
using Mkh.Module.Abstractions;

namespace Mkh.Config.Core;

internal class DefaultConfigProvider : IConfigProvider
{
    private readonly Dictionary<RuntimeTypeHandle, IConfig> _configs = new();

    private readonly IConfiguration _cfg;

    public DefaultConfigProvider(IConfiguration cfg)
    {
        _cfg = cfg;
    }

    public TConfig Get<TConfig>() where TConfig : IConfig, new()
    {
        var item = _configs.FirstOrDefault(m => m.Key.Value == typeof(TConfig).TypeHandle.Value);
        return (TConfig)item.Value;
    }

    /// <summary>
    /// 添加模块配置
    /// </summary>
    /// <param name="modules"></param>
    public void AddModuleConfig(IModuleCollection modules)
    {
        foreach (var module in modules)
        {
            var configType = module.LayerAssemblies.Core.GetTypes().FirstOrDefault(m => typeof(IConfig).IsImplementType(m));
            if (configType != null)
            {
                var instance = (IConfig)Activator.CreateInstance(configType)!;
                _cfg.GetSection($"Mkh:Modules:{module.Code}:Config").Bind(instance);
                _configs.Add(configType.TypeHandle, instance);
            }
        }
    }
}