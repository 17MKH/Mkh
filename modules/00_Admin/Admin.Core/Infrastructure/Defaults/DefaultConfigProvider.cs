using System;
using System.Collections.Generic;
using System.Linq;
using Mkh.Utils.Config;

namespace Mkh.Mod.Admin.Core.Infrastructure.Defaults;

/// <summary>
/// 默认配置提供器
/// </summary>
internal class DefaultConfigProvider : IConfigProvider
{
    public Dictionary<RuntimeTypeHandle, IConfig> Configs = new();

    public TConfig Get<TConfig>() where TConfig : IConfig, new()
    {
        var item = Configs.FirstOrDefault(m => m.Key.Value == typeof(TConfig).TypeHandle.Value);
        if (item.Value != null)
        {
            return (TConfig)item.Value;
        }

        return new TConfig();
    }
}