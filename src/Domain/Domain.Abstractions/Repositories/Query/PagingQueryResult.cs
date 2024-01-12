using System.Collections.Generic;

namespace Mkh.Domain.Abstractions.Repositories.Query;

/// <summary>
/// 分页查询结果
/// </summary>
public class PagingQueryResult<T>
{
    /// <summary>
    /// 数据集
    /// </summary>
    public IEnumerable<T> Rows { get; set; }

    /// <summary>
    /// 总数
    /// </summary>
    public long Total { get; set; }

    /// <summary>
    /// 扩展数据
    /// </summary>
    public object? ExtendedData { get; set; }

    /// <summary>
    /// 创建一个分页查询结果
    /// </summary>
    /// <param name="rows">数据集</param>
    /// <param name="total">总数</param>
    /// <param name="extendedData">扩展数据</param>
    public PagingQueryResult(IEnumerable<T> rows, long total, object? extendedData = null)
    {
        Rows = rows;
        Total = total;
        ExtendedData = extendedData;
    }
}