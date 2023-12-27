using System;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.Accounts;

/// <summary>
/// 账户角色关联信息
/// </summary>
public class AccountRoleRelation : Entity
{
    public Guid AccountId { get; set; }

    public Guid RoleId { get; set; }

}