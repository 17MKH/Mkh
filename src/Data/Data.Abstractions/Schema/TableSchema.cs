using System.Collections.Generic;

namespace Mkh.Data.Abstractions.Schema;

/// <summary>
/// 表结构
/// </summary>
public class TableSchema
{
    /// <summary>
    /// 表名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 列集合
    /// </summary>
    public List<ColumnSchema> Columns { get; set; }
}