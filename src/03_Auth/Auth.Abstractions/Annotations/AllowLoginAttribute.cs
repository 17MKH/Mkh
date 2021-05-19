using System;

namespace Mkh.Auth.Abstractions.Annotations
{
    /// <summary>
    /// 允许登录访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AllowLoginAttribute : Attribute
    {
    }
}
