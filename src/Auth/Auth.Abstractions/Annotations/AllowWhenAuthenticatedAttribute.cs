using System;

namespace Mkh.Auth.Abstractions.Annotations;

/// <summary>
/// 只要认证就能访问，无需授权
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AllowWhenAuthenticatedAttribute : Attribute
{
}