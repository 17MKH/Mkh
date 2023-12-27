using System;
using System.ComponentModel.DataAnnotations;

namespace Mkh.Mod.Admin.Core.Application.Accounts.Dto;

/// <summary>
/// 账户新增模型
/// </summary>
public class AccountAddDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required(ErrorMessage = "请输入用户名")]
    public string Username { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 姓名或名称
    /// </summary>
    [Required(ErrorMessage = "请输入姓名或名称")]
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 绑定的角色数组
    /// </summary>
    public Guid[] RoleIds { get; set; }
}