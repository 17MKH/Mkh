namespace Mkh.Module.Abstractions.Options;

/// <summary>
/// 模块配置项
/// </summary>
public class ModuleOptions
{
    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 加载顺序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 模块配置文件所在目录
    /// </summary>
    public string Dir { get; set; }

    /// <summary>
    /// 数据库配置项
    /// </summary>
    public ModuleDbOptions Db { get; set; }
}