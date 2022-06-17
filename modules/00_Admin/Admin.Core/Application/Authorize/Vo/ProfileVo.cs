using System;
using System.Collections.Generic;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.Authorize.Vo;

/// <summary>
/// 个人信息
/// </summary>
public class ProfileVo
{
    /// <summary>
    /// 账户编号
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// 角色编码
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// 平台
    /// </summary>
    public int Platform { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
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
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 皮肤设置
    /// </summary>
    public ProfileSkinVo Skin { get; set; }

    /// <summary>
    /// 菜单列表
    /// </summary>
    public IList<ProfileMenuVo> Menus { get; set; }

    /// <summary>
    /// 按钮编码列表
    /// </summary>
    public IList<string> Buttons { get; set; } = new List<string>();

    /// <summary>
    /// 详情信息(用于扩展登录对象信息)
    /// </summary>
    public object Details { get; set; }
}