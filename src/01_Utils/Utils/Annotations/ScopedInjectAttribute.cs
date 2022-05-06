using System;

namespace Mkh.Utils.Annotations;

/// <summary>
/// Scoped注入(使用该特性的服务系统会自动注入)
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ScopedInjectAttribute : Attribute
{
    /// <summary>
    /// 是否使用自身的类型进行注入
    /// </summary>
    public bool Itself { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ScopedInjectAttribute()
    {
    }

    /// <summary>
    /// 是否使用自身的类型进行注入
    /// </summary>
    /// <param name="itself"></param>
    public ScopedInjectAttribute(bool itself = false)
    {
        Itself = itself;
    }
}