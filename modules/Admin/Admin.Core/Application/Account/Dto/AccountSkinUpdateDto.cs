using System;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.Account.Dto;

/// <summary>
/// 账户皮肤更新
/// </summary>
public class AccountSkinUpdateDto
{
    /// <summary>
    /// 账户编号
    /// </summary>
    [Ignore]
    public Guid AccountId { get; set; }

    /// <summary>
    /// 皮肤名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 皮肤编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 皮肤主题
    /// </summary>
    public string Theme { get; set; }

    /// <summary>
    /// 皮肤尺寸
    /// </summary>
    public string Size { get; set; }
}