using System.Reflection;

namespace Mkh.Data.Abstractions.Descriptors;

/// <summary>
/// 列信息描述符
/// </summary>
public interface IColumnDescriptor
{
    /// <summary>
    /// 列名
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 属性名称
    /// </summary>
    string PropertyName { get; }

    /// <summary>
    /// 列类型名称
    /// </summary>
    string TypeName { get; set; }

    /// <summary>
    /// 默认值
    /// </summary>
    string DefaultValue { get; set; }

    /// <summary>
    /// 属性信息
    /// </summary>
    PropertyInfo PropertyInfo { get; }

    /// <summary>
    /// 是否主键
    /// </summary>
    bool IsPrimaryKey { get; }

    /// <summary>
    /// 长度(为0表示使用最大长度)
    /// </summary>
    int Length { get; set; }

    /// <summary>
    /// 可空
    /// </summary>
    bool Nullable { get; }

    /// <summary>
    /// 整数位
    /// </summary>
    public int Precision { get; }

    /// <summary>
    /// 小数位
    /// </summary>
    int Scale { get; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 禁止在实体变更日志中记录该属性
    /// </summary>
    public bool DisabledEntityChangeLog { get; set; }
}