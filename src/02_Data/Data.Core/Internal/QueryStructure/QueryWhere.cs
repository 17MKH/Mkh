using System.Linq.Expressions;
using Mkh.Data.Abstractions.Queryable;

namespace Mkh.Data.Core.Internal.QueryStructure;

/// <summary>
/// 查询条件
/// </summary>
public class QueryWhere
{
    /// <summary>
    /// 模式
    /// </summary>
    public QueryWhereMode Mode { get; set; }

    /// <summary>
    /// 表达式
    /// </summary>
    public LambdaExpression Lambda { get; set; }

    /// <summary>
    /// SQL语句
    /// </summary>
    public string Sql { get; set; }

    /// <summary>
    /// 子查询的列
    /// </summary>
    public LambdaExpression SubQueryColumn { get; set; }

    /// <summary>
    /// 子查询运算符
    /// </summary>
    public string SubQueryOperator { get; set; }

    /// <summary>
    /// 子查询构造器
    /// </summary>
    public IQueryable SubQueryable { get; set; }

    public QueryWhere(LambdaExpression lambda)
    {
        Mode = QueryWhereMode.Lambda;
        Lambda = lambda;
    }

    public QueryWhere(string sql)
    {
        Mode = QueryWhereMode.Sql;
        Sql = sql;
    }

    public QueryWhere(LambdaExpression subQueryColumn, string subQueryOperator, IQueryable subQueryable)
    {
        SubQueryColumn = subQueryColumn;
        SubQueryOperator = subQueryOperator;
        SubQueryable = subQueryable;
    }
}

/// <summary>
/// 查询条件类型
/// </summary>
public enum QueryWhereMode
{
    /// <summary>
    /// Lambda
    /// </summary>
    Lambda,
    /// <summary>
    /// SQL语句
    /// </summary>
    Sql,
    /// <summary>
    /// 子查询
    /// </summary>
    SubQuery
}