using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.Menu;

/// <summary>
/// 菜单
/// </summary>
public partial class MenuEntity : EntityBase
{
    /// <summary>
    /// 分组编号
    /// </summary>
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
    [Length(1000)]
    public string RouteParams { get; set; }

    /// <summary>
    /// 路由参数
    /// </summary>
    [Length(1000)]
    public string RouteQuery { get; set; }

    /// <summary>
    /// 链接
    /// </summary>
    [Length(300)]
    public string Url { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 图标颜色
    /// </summary>
    [Nullable]
    public string IconColor { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 打开方式
    /// </summary>
    public MenuOpenTarget OpenTarget { get; set; }

    /// <summary>
    /// 对话框宽度
    /// </summary>
    [Nullable]
    public string DialogWidth { get; set; }

    /// <summary>
    /// 对话框高度
    /// </summary>
    [Nullable]
    public string DialogHeight { get; set; }

    /// <summary>
    /// 对话框可全屏
    /// </summary>
    public bool DialogFullscreen { get; set; }

    /// <summary>
    /// 自定义脚本
    /// </summary>
    public string CustomJs { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Nullable]
    public string Remarks { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool Show { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 多语言配置(JSON字符串)
    /// </summary>
    [Nullable]
    [Length(500)]
    public string LocalesConfig { get; set; }
}