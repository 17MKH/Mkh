using System.ComponentModel.DataAnnotations;
using Mkh.Mod.Admin.Core.Domain.Menu;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.Menu.Dto;

/// <summary>
/// 添加菜单
/// </summary>
[ObjectMap(typeof(MenuEntity))]
public class MenuAddDto
{
    /// <summary>
    /// 分组编号
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "请选择菜单分组")]
    public int GroupId { get; set; }

    /// <summary>
    /// 父节点
    /// </summary>
    public int ParentId { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public MenuType Type { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 图标颜色
    /// </summary>
    public string IconColor { get; set; }

    /// <summary>
    /// 路由所属模块编码
    /// </summary>
    public string Module { get; set; }

    /// <summary>
    /// 路由名称(对应前端路由菜单的菜单编码)
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
    /// 链接
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 打开方式
    /// </summary>
    public MenuOpenTarget OpenTarget { get; set; }

    /// <summary>
    /// 对话框宽度
    /// </summary>
    public string DialogWidth { get; set; }

    /// <summary>
    /// 对话框高度
    /// </summary>
    public string DialogHeight { get; set; }

    /// <summary>
    /// 自定义脚本
    /// </summary>
    public string CustomJs { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool Show { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remarks { get; set; }

    /// <summary>
    /// 多语言配置
    /// </summary>
    public MenuLocales Locales { get; set; }
}