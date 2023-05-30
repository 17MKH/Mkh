using System.ComponentModel;
using System.Reflection;

namespace Mkh.Data.Abstractions.Descriptors;

/// <summary>
/// 逐渐类型
/// </summary>
public interface IPrimaryKeyDescriptor
{
    /// <summary>
    /// 列名
    /// </summary>
    string ColumnName { get; }

    /// <summary>
    /// 属性名称
    /// </summary>
    string PropertyName { get; }

    /// <summary>
    /// 主键类型
    /// </summary>
    PrimaryKeyType Type { get; }

    /// <summary>
    /// 属性信息
    /// </summary>
    PropertyInfo PropertyInfo { get; }

    /// <summary>
    /// 是否没有主键
    /// </summary>
    bool IsNo { get; }

    /// <summary>
    /// 是否Int类型主键
    /// </summary>
    bool IsInt { get; }

    /// <summary>
    /// 是否Long类型主键
    /// </summary>
    bool IsLong { get; }

    /// <summary>
    ///  是否Guid类型主键
    /// </summary>
    bool IsGuid { get; }

    /// <summary>
    /// 是否string类型主键
    /// </summary>
    bool IsString { get; }
}

/// <summary>
/// 主键类型
/// </summary>
public enum PrimaryKeyType
{
    /// <summary>
    /// 没有主键
    /// </summary>
    [Description("无")]
    NoPrimaryKey,
    /// <summary>
    /// 整型
    /// </summary>
    [Description("Int")]
    Int,
    /// <summary>
    /// 长整型
    /// </summary>
    [Description("Long")]
    Long,
    /// <summary>
    /// 全球唯一码
    /// </summary>
    [Description("Guid")]
    Guid,
    /// <summary>
    /// 字符型
    /// </summary>
    [Description("String")]
    String
}