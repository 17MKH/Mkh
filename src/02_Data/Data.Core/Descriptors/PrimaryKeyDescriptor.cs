using System.Reflection;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Core.Descriptors;

/// <summary>
/// 逐渐类型
/// </summary>
public class PrimaryKeyDescriptor : IPrimaryKeyDescriptor
{
    /// <summary>
    /// 列名
    /// </summary>
    public string ColumnName { get; }

    /// <summary>
    /// 属性名称
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    /// 主键类型
    /// </summary>
    public PrimaryKeyType Type { get; }

    /// <summary>
    /// 属性信息
    /// </summary>
    public PropertyInfo PropertyInfo { get; }

    /// <summary>
    /// 是否没有主键
    /// </summary>
    public bool IsNo { get; }

    /// <summary>
    /// 是否Int类型主键
    /// </summary>
    public bool IsInt { get; }

    /// <summary>
    /// 是否Long类型主键
    /// </summary>
    public bool IsLong { get; }

    /// <summary>
    ///  是否Guid类型主键
    /// </summary>
    public bool IsGuid { get; }

    /// <summary>
    /// 是否string类型主键
    /// </summary>
    public bool IsString { get; }

    public PrimaryKeyDescriptor()
    {
        Type = PrimaryKeyType.NoPrimaryKey;
    }

    public PrimaryKeyDescriptor(PropertyInfo p)
    {
        PropertyInfo = p;
        var columnAttribute = p.GetCustomAttribute<ColumnAttribute>();
        ColumnName = columnAttribute != null ? columnAttribute.Name : p.Name;
        PropertyName = p.Name;

        if (p.PropertyType.IsInt())
        {
            Type = PrimaryKeyType.Int;
            IsInt = true;
        }
        else if (p.PropertyType.IsLong())
        {
            Type = PrimaryKeyType.Long;
            IsLong = true;
        }
        else if (p.PropertyType.IsGuid())
        {
            Type = PrimaryKeyType.Guid;
            IsGuid = true;
        }
        else if (p.PropertyType.IsString())
        {
            Type = PrimaryKeyType.String;
            IsString = true;
        }
        else
        {
            Type = PrimaryKeyType.NoPrimaryKey;
            IsNo = true;
        }
    }
}