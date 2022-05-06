using System;
using System.ComponentModel;
using System.Reflection;
using Mkh.Utils.Annotations;

namespace Mkh.Utils.Helpers;

/// <summary>
/// 特性帮助类
/// </summary>
[SingletonInject]
public class AttributeHelper
{
    /// <summary>
    /// 获取DescriptionAttribute的值
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetDescription(TypeInfo type)
    {
        DescriptionAttribute descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute));
        if (descriptionAttribute != null && descriptionAttribute.Description.NotNull())
        {
            return descriptionAttribute.Description;
        }
        return null;
    }

    /// <summary>
    /// 获取DescriptionAttribute的值
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public string GetDescription(MemberInfo member)
    {
        DescriptionAttribute descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(member, typeof(DescriptionAttribute));
        if (descriptionAttribute != null && descriptionAttribute.Description.NotNull())
        {
            return descriptionAttribute.Description;
        }
        return null;
    }

    /// <summary>
    /// 获取DescriptionAttribute的值
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    public string GetDescription(MethodInfo method)
    {
        DescriptionAttribute descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(method, typeof(DescriptionAttribute));
        if (descriptionAttribute != null && descriptionAttribute.Description.NotNull())
        {
            return descriptionAttribute.Description;
        }
        return null;
    }
}