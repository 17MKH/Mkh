using System;

namespace Mkh.Utils.Annotations;

/// <summary>
/// 单例注入(使用该特性的服务系统会自动注入)
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SingletonInjectAttribute : Attribute
{
    /// <summary>
    /// 是否使用自身的类型进行注入，而不是继承的接口
    /// </summary>
    public bool Itself { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public SingletonInjectAttribute()
    {
        Itself = false;
    }

    /// <summary>
    /// 是否使用自身的类型进行注入，而不是继承的接口
    /// </summary>
    /// <param name="itself"></param>
    public SingletonInjectAttribute(bool itself)
    {
        Itself = itself;
    }
}