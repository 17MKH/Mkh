namespace Mkh.Config.Abstractions;

/// <summary>
/// 配置提供器
/// </summary>
public interface IConfigProvider
{
    /// <summary>
    /// 获取指定的配置实例
    /// </summary>
    /// <typeparam name="TConfig"></typeparam>
    /// <returns></returns>
    TConfig Get<TConfig>() where TConfig : IConfig, new();
}