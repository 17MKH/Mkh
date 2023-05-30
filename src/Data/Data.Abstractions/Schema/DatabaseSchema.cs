using System.Collections.Generic;

namespace Mkh.Data.Abstractions.Schema;

/// <summary>
/// 数据库结构
/// </summary>
public class DatabaseSchema
{
    /// <summary>
    /// 数据库名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 表集合
    /// </summary>
    public List<TableSchema> Tables { get; set; }
}