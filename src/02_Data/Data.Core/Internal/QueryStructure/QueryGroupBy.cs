namespace Mkh.Data.Core.Internal.QueryStructure;

/// <summary>
/// 分组查询信息
/// </summary>
public class QueryGroupBy
{
    /// <summary>
    /// 分组字段名称
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// 分组字段表别名
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// 连接信息
    /// </summary>
    public QueryJoin Join { get; set; }

}