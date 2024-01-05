using System.ComponentModel.DataAnnotations;

namespace Mkh.Mod.Admin.Core.Application.Roles.Dto;

/// <summary>
/// 角色添加
/// </summary>
public class RoleCreateDto
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 角色编码
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }
}