using System;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.Accounts;

/// <summary>
/// 账户角色关联信息
/// </summary>
public class AccountRoleRelation : Entity
{
    /// <summary>
    /// 账户编号
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// 角色编号
    /// </summary>
    public Guid RoleId { get; set; }
}