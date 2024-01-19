using System;

namespace Mkh.Utils.Generators;

/// <summary>
/// Guid生成器
/// </summary>
public interface IGuidGenerator
{
    /// <summary>
    /// 生成一个Guid
    /// </summary>
    /// <returns></returns>
    Guid Create();
}

/// <summary>
/// 有序Guid类型
/// </summary>
public enum SequentialGuidType
{
    /// <summary>
    /// MySQL、PostgreSQL、SQLite使用
    /// </summary>
    SequentialAsString,
    /// <summary>
    /// Oracle使用
    /// </summary>
    SequentialAsBinary,
    /// <summary>
    /// SQL Server使用
    /// </summary>
    SequentialAtEnd
}