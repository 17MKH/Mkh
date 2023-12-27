using System;
using System.ComponentModel.DataAnnotations;
using Mkh.Utils.Annotations;
using Mkh.Utils.Validations;

namespace Mkh.Mod.Admin.Core.Application.Accounts.Dto;

[ObjectMap(typeof(AccountEntity), true)]
public class AccountUpdateDto : AccountAddDto
{
    [Required(ErrorMessage = "请选择账户")]
    [GuidNotEmptyValidation(ErrorMessage = "请选择账户")]
    public Guid Id { get; set; }
}