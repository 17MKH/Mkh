using System.Collections.Generic;

namespace Mkh.Data.Abstractions.Query;

/// <summary>
/// 查询分页
/// </summary>
public class QueryPagingDto
{
    /// <summary>
    /// 当前页
    /// </summary>
    public int Index { get; set; } = 1;

    /// <summary>
    /// 页大小
    /// </summary>
    public int Size { get; set; } = 15;

    /// <summary>
    /// 排序字段
    /// </summary>
    public List<QuerySortDto> Sort { get; set; }
}