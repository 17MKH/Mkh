using System;

namespace Mkh.Auth.Abstractions.Annotations
{
    /// <summary>
    /// 禁用审计功能
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class DisableAuditAttribute : Attribute
    {
    }
}
