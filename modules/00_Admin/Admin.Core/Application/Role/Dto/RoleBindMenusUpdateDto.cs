using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mkh.Mod.Admin.Core.Domain.Menu;

namespace Mkh.Mod.Admin.Core.Application.Role.Dto;

/// <summary>
/// 角色绑定菜单更新
/// </summary>
public class RoleBindMenusUpdateDto
{
    /// <summary>
    /// 角色编号
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "请选择角色")]
    public int RoleId { get; set; }

    /// <summary>
    /// 绑定的菜单列表
    /// </summary>
    public IList<BindMenuUpdateDto> Menus { get; set; }
}

public class BindMenuUpdateDto
{
    /// <summary>
    /// 菜单编码
    /// </summary>
    public int MenuId { get; set; }

    /// <summary>
    /// 菜单类型
    /// </summary>
    public MenuType MenuType { get; set; }

    /// <summary>
    /// 按钮编码列表
    /// </summary>
    public IList<string> Buttons { get; set; }

    /// <summary>
    /// 绑定的权限编码列表
    /// </summary>
    public IList<string> Permissions { get; set; }
}