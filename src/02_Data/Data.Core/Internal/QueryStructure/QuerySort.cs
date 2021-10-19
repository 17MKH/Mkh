using System.Linq.Expressions;
using Mkh.Data.Abstractions.Pagination;

namespace Mkh.Data.Core.Internal.QueryStructure;

/// <summary>
/// 查询排序
/// </summary>
public class QuerySort
{
    /// <summary>
    /// 模式
    /// </summary>
    public QuerySortMode Mode { get; set; }

    /// <summary>
    /// 表达式
    /// </summary>
    public LambdaExpression Lambda { get; set; }

    /// <summary>
    /// SQL语句
    /// </summary>
    public string Sql { get; set; }

    /// <summary>
    /// 排序类型
    /// </summary>
    public SortType Type { get; set; }
}

/// <summary>
/// 查询排序模式
/// </summary>
public enum QuerySortMode
{
    /// <summary>
    /// 拉姆达表达式
    /// </summary>
    Lambda,
    /// <summary>
    /// SQL语句
    /// </summary>
    Sql
}