using System;
using Mkh.Domain.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.Accounts;

/// <summary>
/// 账户配置
/// </summary>
public class AccountConfig : Entity
{
    /// <summary>
    /// 账户编号
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// 皮肤名称
    /// </summary>
    public string SkinName { get; set; }

    /// <summary>
    /// 皮肤编码
    /// </summary>
    public string SkinCode { get; set; }

    /// <summary>
    /// 皮肤主题
    /// </summary>
    public string SkinTheme { get; set; }

    /// <summary>
    /// 皮肤尺寸
    /// </summary>
    public string SkinSize { get; set; }
}