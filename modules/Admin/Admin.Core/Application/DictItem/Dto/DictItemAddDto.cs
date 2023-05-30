using Mkh.Mod.Admin.Core.Domain.DictItem;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.DictItem.Dto;

[ObjectMap(typeof(DictItemEntity))]
public class DictItemAddDto
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
    public string Name { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// 扩展数据
    /// </summary>
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