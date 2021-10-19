using System;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Abstractions.Options;

/// <summary>
/// 代码优先配置项
/// </summary>
public class CodeFirstOptions
{
    /// <summary>
    /// 自定义代码优先提供器
    /// </summary>
    public ICodeFirstProvider CustomCodeFirstProvider { get; set; }

    /// <summary>
    /// 是否创建库
    /// </summary>
    public bool CreateDatabase { get; set; }

    /// <summary>
    /// 是否更新列
    /// </summary>
    public bool UpdateColumn { get; set; }

    /// <summary>
    /// 是否初始化数据
    /// </summary>
    public bool InitData { get; set; }

    /// <summary>
    /// 初始化数据的文件路径
    /// </summary>
    public string InitDataFilePath { get; set; }

    /// <summary>
    /// 创建数据库前事件
    /// </summary>
    public Action<IDbContext> BeforeCreateDatabase { get; set; }

    /// <summary>
    /// 创建数据库后事件
    /// </summary>
    public Action<IDbContext> AfterCreateDatabase { get; set; }

    /// <summary>
    /// 创建表前事件
    /// </summary>
    public Action<IDbContext, IEntityDescriptor> BeforeCreateTable { get; set; }

    /// <summary>
    /// 创建表后事件
    /// </summary>
    public Action<IDbContext, IEntityDescriptor> AfterCreateTable { get; set; }
}