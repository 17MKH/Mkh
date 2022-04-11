using System.Diagnostics;
using System.Text.Json.Serialization;
using Mkh.Mod.Admin.Core.Application.Authorize.Vo;
using Mkh.Utils.Annotations;
using Mkh.Utils.Json;

namespace Mkh.Mod.Admin.Core.Domain.Menu;

public partial class MenuEntity
{
    /// <summary>
    /// 类型名称
    /// </summary>
    [Ignore]
    public string TypeName => Type.ToDescription();

    /// <summary>
    /// 打开方式名称
    /// </summary>
    [Ignore]
    public string OpenTargetName => OpenTarget.ToDescription();

    /// <summary>
    /// 多语言配置
    /// </summary>
    public MenuLocales Locales
    {
        get
        {
            if (LocalesConfig.NotNull())
            {
                return JsonHelper.Instance.Deserialize<MenuLocales>(LocalesConfig);
            }

            return new MenuLocales();
        }
    }
}

public class MenuLocales
{
    /// <summary>
    /// 中文
    /// </summary>
    [JsonPropertyName("zh-cn")]
    public string ZhCN { get; set; }

    /// <summary>
    /// 英文
    /// </summary>
    public string En { get; set; }
}