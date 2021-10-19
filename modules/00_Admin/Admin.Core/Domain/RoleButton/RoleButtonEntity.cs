using Mkh.Data.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.RoleButton;

/// <summary>
/// 角色按钮绑定关系
/// </summary>
public class RoleButtonEntity : Entity
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
    /// 按钮编码
    /// </summary>
    public string ButtonCode { get; set; }
}