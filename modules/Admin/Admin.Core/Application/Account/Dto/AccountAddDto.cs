using System.ComponentModel.DataAnnotations;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.Account.Dto;

/// <summary>
/// 账户新增模型
/// </summary>
[ObjectMap(typeof(AccountEntity))]
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
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 绑定角色
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "请选择角色")]
    public int RoleId { get; set; }
}