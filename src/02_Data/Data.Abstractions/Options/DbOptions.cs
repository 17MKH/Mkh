using Mkh.Data.Abstractions.Adapter;

namespace Mkh.Data.Abstractions.Options;

/// <summary>
/// 数据库配置
/// </summary>
public class DbOptions
{
    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// 数据库提供器
    /// </summary>
    public DbProvider Provider { get; set; }

    /// <summary>
    /// 是否开启日志
    /// </summary>
    public bool Log { get; set; }

    /// <summary>
    /// 数据库版本
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// 表名称前缀
    /// </summary>
    public string TableNamePrefix { get; set; }

    /// <summary>
    /// 表名称分隔符
    /// </summary>
    public string TableNameSeparator { get; set; } = "_";

    /// <summary>
    /// 模块编码(17MKH专属属性)
    /// </summary>
    public string ModuleCode { get; set; }
}