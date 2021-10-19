using System.Collections.Generic;

namespace Mkh.Data.Abstractions.Query;

/// <summary>
/// 查询结果模型
/// </summary>
/// <typeparam name="T"></typeparam>
public class QueryResultModel<T>
{
    /// <summary>
    /// 总数
    /// </summary>
    public long Total { get; set; }

    /// <summary>
    /// 数据集
    /// </summary>
    public IList<T> Rows { get; set; }

    /// <summary>
    /// 扩展数据
    /// </summary>
    public object Data { get; set; }

    public QueryResultModel(IList<T> rows, long total)
    {
        Rows = rows;
        Total = total;
    }
}