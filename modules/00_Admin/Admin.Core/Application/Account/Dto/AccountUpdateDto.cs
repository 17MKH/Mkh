using System;
using System.ComponentModel.DataAnnotations;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Utils.Annotations;
using Mkh.Utils.Validations;

namespace Mkh.Mod.Admin.Core.Application.Account.Dto;

[ObjectMap(typeof(AccountEntity), true)]
public class AccountUpdateDto : AccountAddDto
{
    [Required(ErrorMessage = "请选择账户")]
    [GuidNotEmptyValidation(ErrorMessage = "请选择账户")]
    public Guid Id { get; set; }
}