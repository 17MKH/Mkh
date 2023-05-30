using System.Collections.Generic;
using Mkh.Mod.Admin.Core.Application.Menu.Dto;
using Mkh.Mod.Admin.Core.Domain.Menu;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.Authorize.Vo;

/// <summary>
/// 个人信息菜单信息
/// </summary>
[ObjectMap(typeof(MenuEntity), true)]
public class ProfileMenuVo
{
    /// <summary>
    /// 编号
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 父节点编号
    /// </summary>
    public int ParentId { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public MenuType Type { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string RouteName { get; set; }

    /// <summary>
    /// 路由参数
    /// </summary>
    public string RouteParams { get; set; }

    /// <summary>
    /// 路由参数
    /// </summary>
    public string RouteQuery { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 图标颜色
    /// </summary>
    public string IconColor { get; set; }

    /// <summary>
    /// 链接
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 链接菜单打开方式
    /// </summary>
    public MenuOpenTarget Target { get; set; }

    /// <summary>
    /// 对话框宽度
    /// </summary>
    public string DialogWidth { get; set; }

    /// <summary>
    /// 对话框高度
    /// </summary>
    public string DialogHeight { get; set; }

    /// <summary>
    /// 对话框可全屏
    /// </summary>
    public bool DialogFullscreen { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool Show { get; set; }

    /// <summary>
    /// 按钮编码列表
    /// </summary>
    public List<string> Buttons { get; set; }

    /// <summary>
    /// 多语言配置
    /// </summary>
    public MenuLocales Locales { get; set; }

    /// <summary>
    /// 子菜单
    /// </summary>
    public List<ProfileMenuVo> Children { get; set; }
}

