using System;

namespace Mkh.Utils.Annotations;

/// <summary>
/// 映射
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ObjectMapAttribute : Attribute
{
    /// <summary>
    /// 要映射的类型
    /// </summary>
    public Type TargetType { get; set; }

    /// <summary>
    /// 双向映射
    /// </summary>
    public bool TwoWay { get; set; }

    /// <summary>
    /// 创建AutoMapper的映射管理
    /// </summary>
    /// <param name="targetType">映射目标类型</param>
    /// <param name="twoWay">是否双向映射</param>
    public ObjectMapAttribute(Type targetType, bool twoWay = false)
    {
        TargetType = targetType;
        TwoWay = twoWay;
    }
}