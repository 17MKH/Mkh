using Mkh.Data.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.RolePermission;

/// <summary>
/// 角色权限绑定关系
/// </summary>
public class RolePermissionEntity : Entity
{
    /// <summary>
    /// 角色编号
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// 菜单组编号
    /// </summary>
    public int MenuGroupId { get; set; }

    /// <summary>
    /// 菜单编号
    /// </summary>
    public int MenuId { get; set; }

    /// <summary>
    /// 权限编码
    /// </summary>
    public string PermissionCode { get; set; }
}