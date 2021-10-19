using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.MenuGroup;

/// <summary>
/// 菜单组
/// </summary>
public class MenuGroupEntity : EntityBase
{
    /// <summary>
    /// 分组名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Length(300)]
    [Nullable]
    public string Remarks { get; set; }
}