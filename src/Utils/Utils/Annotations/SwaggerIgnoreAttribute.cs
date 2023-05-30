using System;

namespace Mkh.Utils.Annotations;

/// <summary>
/// 在Swagger中忽略该字段或属性
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SwaggerIgnoreAttribute : Attribute
{
}