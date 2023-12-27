using System.ComponentModel.DataAnnotations;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.Role.Dto;

/// <summary>
/// 角色更新
/// </summary>
[ObjectMap(typeof(Domain.Roles.Role), true)]
public class RoleUpdateDto : RoleAddDto
{
    [Required(ErrorMessage = "请选择要修改的角色")]
    [Range(1, int.MaxValue, ErrorMessage = "请选择要修改的角色")]
    public int Id { get; set; }
}