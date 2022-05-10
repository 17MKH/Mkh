using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Core.Descriptors;

/// <summary>
/// 列信息描述符
/// </summary>
internal class ColumnDescriptor : IColumnDescriptor
{
    /// <summary>
    /// 列名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 属性名称
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// 列类型名称
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// 默认值
    /// </summary>
    public string DefaultValue { get; set; }

    /// <summary>
    /// 属性信息
    /// </summary>
    public PropertyInfo PropertyInfo { get; }

    /// <summary>
    /// 是否主键
    /// </summary>
    public bool IsPrimaryKey { get; }

    /// <summary>
    /// 长度(为0表示使用最大长度)
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// 可空
    /// </summary>
    public bool Nullable { get; }

    /// <summary>
    /// 精度位数
    /// </summary>
    public int Precision { get; }

    /// <summary>
    /// 精度小数
    /// </summary>
    public int Scale { get; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 禁止在实体变更日志中记录该属性
    /// </summary>
    public bool DisabledEntityChangeLog { get; set; }

    public ColumnDescriptor(PropertyInfo property, IDbAdapter dbAdapter)
    {
        if (property == null)
            return;

        PropertyInfo = property;
        PropertyName = property.Name;
        Description = "";

        //判断是否有主键
        IsPrimaryKey = Attribute.GetCustomAttributes(property).Any(attr => attr.GetType() == typeof(KeyAttribute));

        if (!IsPrimaryKey)
        {
            IsPrimaryKey = property.Name.EqualsIgnoreCase("Id");
        }

        if (property.PropertyType.IsNullable())
        {
            Nullable = true;
        }
        else
        {
            var nullableAtt = property.GetCustomAttribute<NullableAttribute>();
            Nullable = nullableAtt != null;
        }

        var precisionAtt = property.GetCustomAttribute<PrecisionAttribute>();
        if (precisionAtt != null)
        {
            Precision = precisionAtt.M;
            Scale = precisionAtt.D;
        }

        //获取列长度
        var lengthAtt = property.GetCustomAttribute<LengthAttribute>();
        Length = lengthAtt == null || lengthAtt.Length < 0 ? 50 : lengthAtt.Length;

        //解析列
        dbAdapter.ResolveColumn(this);

        //自定义列信息
        var columnAttribute = PropertyInfo.GetCustomAttribute<ColumnAttribute>();
        if (columnAttribute != null)
        {
            if (columnAttribute.Name.NotNull())
                Name = columnAttribute.Name;

            if (columnAttribute.Type.NotNull())
                TypeName = columnAttribute.Type;

            if (columnAttribute.DefaultValue.NotNull())
                DefaultValue = columnAttribute.DefaultValue;

            if (columnAttribute.Description.NotNull())
                Description = columnAttribute.Description;
        }

        DisabledEntityChangeLog = PropertyInfo.GetCustomAttribute<IgnoreOnEntityEvent>() != null;

        //如果未自定义列名，则使用属性名称
        if (Name.IsNull())
            Name = property.Name;

        //字段说明
        var descriptionAttribute = PropertyInfo.GetCustomAttribute<DescriptionAttribute>();
        if (descriptionAttribute != null)
        {
            Description = descriptionAttribute.Description;
        }
    }
}