using System;
using System.ComponentModel;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;
using Mkh.Excel.Abstractions.Annotations;

namespace Mkh.Mod.Admin.Core.Domain.Account;

/// <summary>
/// 账户信息
/// </summary>
[EnableEntityAllEvent]
public partial class AccountEntity : EntityBaseSoftDelete<Guid>, ITenant
{
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 角色编号
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [IgnoreOnExcelExport]
    public string Password { get; set; }

    /// <summary>
    /// 用户姓名或者企业名称，具体是什么与业务有关
    /// </summary>
    [Length(250)]
    [Description("姓名")]
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [Length(20)]
    [Nullable]
    [Description("手机号")]
    public string Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Length(300)]
    [Nullable]
    [Description("邮箱")]
    public string Email { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Description("状态")]
    public AccountStatus Status { get; set; }

    /// <summary>
    /// 激活时间
    /// </summary>
    public DateTime ActivatedTime { get; set; } = DateTime.Now;
}

/// <summary>
/// 账户状态
/// </summary>
public enum AccountStatus
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")]
    UnKnown = -1,
    /// <summary>
    /// 注册
    /// </summary>
    [Description("注册")]
    Register,
    /// <summary>
    /// 激活
    /// </summary>
    [Description("激活")]
    Active,
    /// <summary>
    /// 禁用
    /// </summary>
    [Description("禁用")]
    Disabled
}