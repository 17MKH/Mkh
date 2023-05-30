using System.Linq.Expressions;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Core.Internal.QueryStructure;

/// <summary>
/// 查询链接信息
/// </summary>
public class QueryJoin
{
    /// <summary>
    /// 连接类型
    /// </summary>
    public JoinType Type { get; set; }

    /// <summary>
    /// 别名
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// 连接条件
    /// </summary>
    public LambdaExpression On { get; set; }

    /// <summary>
    /// 表名称
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// 实体信息
    /// </summary>
    public IEntityDescriptor EntityDescriptor { get; set; }

    /// <summary>
    /// 针对SqlServer的NoLock特性
    /// </summary>
    public bool NoLock { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entityDescriptor">实体描述符</param>
    /// <param name="alias">别名</param>
    /// <param name="type"></param>
    /// <param name="on">表连接表达式</param>
    /// <param name="noLock"></param>
    public QueryJoin(IEntityDescriptor entityDescriptor, string alias, JoinType type = JoinType.UnKnown, LambdaExpression on = null, bool noLock = true)
    {
        EntityDescriptor = entityDescriptor;
        Alias = alias;
        TableName = entityDescriptor.TableName;
        Type = type;
        On = on;
        NoLock = noLock;
    }
}

/// <summary>
/// 表连接类型
/// </summary>
public enum JoinType
{
    UnKnown,
    Left,
    Inner,
    Right
}