namespace Mkh.Data.Abstractions.Pagination;

/// <summary>
/// 排序规则
/// </summary>
public class Sort
{
    /// <summary>
    /// 排序字段
    /// </summary>
    public string Field { get; }

    /// <summary>
    /// 排序方式
    /// </summary>
    public SortType Type { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="field">排序字段</param>
    public Sort(string field)
    {
        Field = field;
        Type = SortType.Asc;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="field">排序字段</param>
    /// <param name="type"></param>
    public Sort(string field, SortType type)
    {
        Field = field;
        Type = type;
    }
}

/// <summary>
/// 排序规则
/// </summary>
public enum SortType
{
    /// <summary>
    /// 升序
    /// </summary>
    Asc,
    /// <summary>
    /// 降序
    /// </summary>
    Desc
}