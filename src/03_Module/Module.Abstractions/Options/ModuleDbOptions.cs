using Mkh.Data.Abstractions.Adapter;

namespace Mkh.Module.Abstractions.Options;

/// <summary>
/// 模块数据库配置项
/// </summary>
public class ModuleDbOptions
{
    /// <summary>
    /// 数据库提供器
    /// </summary>
    public DbProvider Provider { get; set; }

    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; }

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
    /// 启用代码优先
    /// </summary>
    public bool CodeFirst { get; set; } = true;

    /// <summary>
    /// 代码优先是否创建库
    /// </summary>
    public bool CreateDatabase { get; set; } = true;

    /// <summary>
    /// 代码优先是否更新列
    /// </summary>
    public bool UpdateColumn { get; set; } = true;

    /// <summary>
    /// 是否创建数据库后初始化数据
    /// </summary>
    public bool InitData { get; set; } = true;
}