using Mkh.Domain.Abstractions.Repositories.Paging;

namespace Mkh.Domain.Abstractions.Repositories.Query;

/// <summary>
/// 分页查询基类
/// </summary>
public abstract class PagingQueryBase
{
    /// <summary>
    /// 当前页
    /// </summary>
    public int PageIndex { get; set; } = 1;

    /// <summary>
    /// 页大小
    /// </summary>
    public int PageSize { get; set; } = 15;

    /// <summary>
    /// 排序规则
    /// </summary>
    public Sort[]? OrderBy { get; set; }
}