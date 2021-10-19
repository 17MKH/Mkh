using System;

namespace Mkh.Data.Abstractions.Annotations;

/// <summary>
/// 主键
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class KeyAttribute : Attribute
{
}