using Mkh.Data.Abstractions.Entities;
using Mkh.Mod.Admin.Core.Domain.Menu;

namespace Mkh.Mod.Admin.Core.Domain.RoleMenu;

/// <summary>
/// 角色菜单绑定信息
/// </summary>
public class RoleMenuEntity : Entity
{
    /// <summary>
    /// 角色编号
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// 菜单分组编号
    /// </summary>
    public int MenuGroupId { get; set; }

    /// <summary>
    /// 菜单编号
    /// </summary>
    public int MenuId { get; set; }

    /// <summary>
    /// 菜单类型
    /// </summary>
    public MenuType MenuType { get; set; }
}