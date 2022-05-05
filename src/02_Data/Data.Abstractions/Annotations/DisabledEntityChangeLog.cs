using System;

namespace Mkh.Data.Abstractions.Annotations
{
    /// <summary>
    /// 禁止在实体变更日志中记录该属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DisabledEntityChangeLog : Attribute
    {
    }
}
