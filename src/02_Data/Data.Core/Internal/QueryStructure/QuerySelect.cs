using System.Linq.Expressions;

namespace Mkh.Data.Core.Internal.QueryStructure;

/// <summary>
/// 查询选择信息
/// </summary>
public class QuerySelect
{
    /// <summary>
    /// 模式
    /// </summary>
    public QuerySelectMode Mode { get; set; }

    /// <summary>
    /// 包含列
    /// </summary>
    public LambdaExpression Include { get; set; }

    /// <summary>
    /// 排除列
    /// </summary>
    public LambdaExpression Exclude { get; set; }

    /// <summary>
    /// 原生SQL
    /// </summary>
    public string Sql { get; set; }

    /// <summary>
    /// 函数计算表达式
    /// </summary>
    public LambdaExpression FunctionExpression { get; set; }

    /// <summary>
    /// 函数名称
    /// </summary>
    public string FunctionName { get; set; }
}

/// <summary>
/// 查询选择列模式
/// </summary>
public enum QuerySelectMode
{
    /// <summary>
    /// UnKnown
    /// </summary>
    UnKnown,
    /// <summary>
    /// Lambda
    /// </summary>
    Lambda,
    /// <summary>
    /// SQL语句
    /// </summary>
    Sql,
    /// <summary>
    /// 函数
    /// </summary>
    Function
}