using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Mkh.Utils.Web;

/// <summary>
/// 控制器描述符
/// </summary>
public class ControllerDescriptor
{
    /// <summary>
    /// 区域
    /// </summary>
    public string Area { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 操作列表
    /// </summary>
    public List<ActionDescriptor> Actions { get; set; }

    /// <summary>
    /// 类型信息
    /// </summary>
    [JsonIgnore]
    public TypeInfo TypeInfo { get; set; }
}