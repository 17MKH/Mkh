using System;

namespace Mkh.Data.Abstractions.Annotations;

/// <summary>
/// 列名，指定实体属性在表中对应的列名
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute : Attribute
{
    /// <summary>
    /// 列名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 列类型名称
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 默认值
    /// </summary>
    public string DefaultValue { get; set; }

    /// <summary>
    /// 列说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="description">列说明</param>
    /// <param name="type">列类型</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="name">列名</param>
    public ColumnAttribute(string description, string defaultValue = null, string type = null, string name = null)
    {
        Description = description;
        DefaultValue = defaultValue;
        Type = type;
        Name = name;
    }
}