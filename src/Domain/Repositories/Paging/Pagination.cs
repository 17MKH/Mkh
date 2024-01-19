using System.Collections.Generic;

namespace Mkh.Domain.Repositories.Paging;

/// <summary>
/// 分页类
/// </summary>
public class Pagination
{
    /// <summary>
    /// 创建分页对象、默认当前页为1，页大小为15
    /// </summary>
    public Pagination() : this(1, 15)
    {

    }

    /// <summary>
    /// 创建分页对象
    /// </summary>
    /// <param name="index">当前页</param>
    /// <param name="size">页大小</param>
    public Pagination(int index, int size)
    {
        _index = index;
        _size = size;
    }

    private int _index;
    private int _size;
    private readonly int _defaultSize = 15;

    /// <summary>
    /// 页码
    /// </summary>
    public int Index
    {
        get => _index < 1 ? 1 : _index;
        set => _index = value;
    }

    /// <summary>
    /// 页大小
    /// </summary>
    public int Size
    {
        get => _size < 1 || _size > MaxSize ? _defaultSize : _size;
        set => _size = value;
    }

    /// <summary>
    /// 页大小最大值，防止失误返回过大数据
    /// </summary>
    public int MaxSize { get; set; } = 1000;

    /// <summary>
    /// 跳过数量
    /// </summary>
    public int Skip => (Index - 1) * Size;

    /// <summary>
    /// 总数量
    /// </summary>
    public long TotalCount { get; set; }

    /// <summary>
    /// 是否查询总数
    /// </summary>
    public bool IsQueryTotalCount { get; set; } = true;

    /// <summary>
    /// 总页数
    /// </summary>
    public long TotalPage => (TotalCount - 1 + Size) / Size;

    /// <summary>
    /// 排序
    /// </summary>
    public List<Sort> OrderBy { get; } = new();
}
