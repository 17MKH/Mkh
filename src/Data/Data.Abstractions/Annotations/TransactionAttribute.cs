using System;
using System.Data;

namespace Mkh.Data.Abstractions.Annotations;

/// <summary>
/// 特性事务
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class TransactionAttribute : Attribute
{
    /// <summary>
    /// 事务隔离级别
    /// </summary>
    public IsolationLevel? IsolationLevel { get; set; }
}