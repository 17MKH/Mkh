using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.DictItem;

/// <summary>
/// 数据字典项实体
/// </summary>
public class DictItemEntity : EntityBaseSoftDelete
{
    /// <summary>
    /// 分组编码
    /// </summary>
    public string GroupCode { get; set; }

    /// <summary>
    /// 字典编码
    /// </summary>
    public string DictCode { get; set; }

    /// <summary>
    /// 父节点
    /// </summary>
    public int ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Length(100)]
    public string Name { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    [Length(100)]
    public string Value { get; set; }

    /// <summary>
    /// 扩展数据
    /// </summary>
    [Nullable]
    [Length(0)]
    public string Extend { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 级别
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}