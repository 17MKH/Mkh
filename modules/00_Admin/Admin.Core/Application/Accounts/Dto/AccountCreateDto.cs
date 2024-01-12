using System;

namespace Mkh.Mod.Admin.Core.Application.Accounts.Dto;

/// <summary>
/// 账户新增模型
/// </summary>
public class AccountCreateDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// 姓名或名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 绑定的角色数组
    /// </summary>
    public required Guid[] RoleIds { get; set; }
}