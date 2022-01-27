using Mkh.Config.Abstractions;

namespace Mkh.Config.Core;

internal class ConfigProvider : IConfigProvider
{
    private readonly Dictionary<RuntimeTypeHandle, IConfig> _configs = new();

    public void Add<TConfig>(TConfig config) where TConfig : IConfig
    {
        _configs.Add(typeof(TConfig).TypeHandle, config);
    }

    public TConfig Get<TConfig>() where TConfig : IConfig, new()
    {
        var item = _configs.FirstOrDefault(m => m.Key.Value == typeof(TConfig).TypeHandle.Value);
        return (TConfig)item.Value;
    }
}