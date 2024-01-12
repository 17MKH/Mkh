using System;
using System.ComponentModel.DataAnnotations;
using Mkh.Utils.Validations;

namespace Mkh.Mod.Admin.Core.Application.Roles.Dto;

/// <summary>
/// 角色更新
/// </summary>
public class RoleUpdateDto : RoleCreateDto
{
    [Required]
    [GuidEmptyValidation]
    public Guid Id { get; set; }
}