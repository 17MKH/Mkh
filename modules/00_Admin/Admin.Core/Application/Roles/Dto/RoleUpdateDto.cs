using System;
using System.ComponentModel.DataAnnotations;
using Mkh.Utils.Annotations;
using Mkh.Utils.Validations;

namespace Mkh.Mod.Admin.Core.Application.Roles.Dto;

/// <summary>
/// 角色更新
/// </summary>
[ObjectMap(typeof(Domain.Roles.Role), true)]
public class RoleUpdateDto : RoleCreateDto
{
    [Required]
    [GuidNotEmptyValidation]
    public Guid Id { get; set; }
}