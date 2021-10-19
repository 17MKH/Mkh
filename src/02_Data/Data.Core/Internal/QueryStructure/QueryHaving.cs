using System.Linq.Expressions;

namespace Mkh.Data.Core.Internal.QueryStructure;

/// <summary>
/// 聚合过滤
/// </summary>
public class QueryHaving
{
    /// <summary>
    /// 模式
    /// </summary>
    public QueryHavingMode Mode { get; set; }

    /// <summary>
    /// 表达式
    /// </summary>
    public LambdaExpression Lambda { get; set; }

    /// <summary>
    /// SQL语句
    /// </summary>
    public string Sql { get; set; }


    public QueryHaving(LambdaExpression lambda)
    {
        Mode = QueryHavingMode.Lambda;
        Lambda = lambda;
    }

    public QueryHaving(string sql)
    {
        Mode = QueryHavingMode.Sql;
        Sql = sql;
    }
}

/// <summary>
/// 查询条件类型
/// </summary>
public enum QueryHavingMode
{
    /// <summary>
    /// Lambda
    /// </summary>
    Lambda,
    /// <summary>
    /// SQL语句
    /// </summary>
    Sql
}