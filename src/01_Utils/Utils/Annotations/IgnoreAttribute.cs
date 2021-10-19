using System;

namespace Mkh.Utils.Annotations;

/// <summary>
/// 忽略
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class IgnoreAttribute : Attribute
{
}