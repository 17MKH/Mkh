using System;

namespace Mkh.Data.Abstractions.Annotations;

/// <summary>
/// 表名称，指定实体类在数据库中对应的表名称
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class TableAttribute : Attribute
{
    /// <summary>
    /// 表名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 设置前缀
    /// </summary>
    public bool SetPrefix { get; set; }

    /// <summary>
    /// 自动创建表
    /// </summary>
    public bool AutoCreate { get; set; }

    /// <summary>
    /// 指定实体类在数据库中对应的表名称
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="setPrefix">设置前缀</param>
    /// <param name="autoCreate">自动创建表</param>
    public TableAttribute(string tableName, bool setPrefix = true, bool autoCreate = true)
    {
        Name = tableName;
        SetPrefix = setPrefix;
        AutoCreate = autoCreate;
    }
}