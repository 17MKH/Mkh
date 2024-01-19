using System;
using System.Collections.Generic;
using Mkh.Domain.Values;

namespace Mkh.Mod.Admin.Core.Domain.Accounts;

/// <summary>
/// 账户角色
/// </summary>
public class AccountRole : ValueObject
{
    public Guid AccountId { get; set; }

    public Guid RoleId { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return AccountId;
        yield return RoleId;
    }
}