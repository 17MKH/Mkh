using System;
using System.Collections.Generic;
using Mkh.Domain.Abstractions.Entities;
using Mkh.Mod.Admin.Core.Domain.Roles;

namespace Mkh.Mod.Admin.Core.Domain.Accounts;

/// <summary>
/// 账户信息
/// </summary>
internal class Account : CommonAggregateRoot, ITenant
{
    /// <summary>
    /// 租户编号
    /// </summary>
    public Guid? TenantId { get; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// 关联角色ID
    /// </summary>
    public IEnumerable<Role>? Roles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    public Account(Guid? tenantId, string username, string password)
    {
        TenantId = tenantId;
        Username = username;
        Password = password;
    }
}