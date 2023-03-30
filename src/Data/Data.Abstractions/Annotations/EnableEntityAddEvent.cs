using System;

namespace Mkh.Data.Abstractions.Annotations
{
    /// <summary>
    /// 启用实体新增事件
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EnableEntityAddEvent : Attribute
    {
    }
}
