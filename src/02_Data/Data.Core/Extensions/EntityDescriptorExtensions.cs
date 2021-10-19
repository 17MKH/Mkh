using System.Linq;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Core.Extensions;

internal static class EntityDescriptorExtensions
{
    /// <summary>
    /// 获取已删除字段名称
    /// </summary>
    /// <returns></returns>
    public static string GetDeletedColumnName(this IEntityDescriptor descriptor)
    {
        if (descriptor.IsSoftDelete)
            return GetColumnNameByPropertyName(descriptor, "Deleted");

        return string.Empty;
    }

    /// <summary>
    /// 获取删除时间属性对应字段名称
    /// </summary>
    /// <returns></returns>
    public static string GetDeletedTimeColumnName(this IEntityDescriptor descriptor)
    {
        if (descriptor.IsSoftDelete)
            return GetColumnNameByPropertyName(descriptor, "DeletedTime");

        return string.Empty;
    }

    /// <summary>
    /// 获取删除人属性对应字段名称
    /// </summary>
    /// <returns></returns>
    public static string GetDeletedByColumnName(this IEntityDescriptor descriptor)
    {
        if (descriptor.IsSoftDelete)
            return GetColumnNameByPropertyName(descriptor, "DeletedBy");

        return string.Empty;
    }

    /// <summary>
    /// 获取删除人名称属性对应字段名称
    /// </summary>
    /// <returns></returns>
    public static string GetDeleterColumnName(this IEntityDescriptor descriptor)
    {
        if (descriptor.IsSoftDelete)
            return GetColumnNameByPropertyName(descriptor, "Deleter");

        return string.Empty;
    }

    /// <summary>
    /// 获取修改人属性对应字段名称
    /// </summary>
    /// <returns></returns>
    public static string GetModifiedByColumnName(this IEntityDescriptor descriptor)
    {
        if (descriptor.IsEntityBase)
            return GetColumnNameByPropertyName(descriptor, "ModifiedBy");

        return string.Empty;
    }

    /// <summary>
    /// 获取修改人属性对应字段名称
    /// </summary>
    /// <returns></returns>
    public static string GetModifierColumnName(this IEntityDescriptor descriptor)
    {
        if (descriptor.IsEntityBase)
            return GetColumnNameByPropertyName(descriptor, "Modifier");

        return string.Empty;
    }

    /// <summary>
    /// 获取修改时间属性对应字段名称
    /// </summary>
    /// <returns></returns>
    public static string GetModifiedTimeColumnName(this IEntityDescriptor descriptor)
    {
        if (descriptor.IsEntityBase)
            return GetColumnNameByPropertyName(descriptor, "ModifiedTime");

        return string.Empty;
    }

    private static string GetColumnNameByPropertyName(this IEntityDescriptor descriptor, string propertyName)
    {
        return descriptor.Columns.First(m => m.PropertyInfo.Name.Equals(propertyName)).Name;
    }
}