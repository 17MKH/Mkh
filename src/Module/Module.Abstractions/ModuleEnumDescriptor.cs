using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mkh.Module.Abstractions;

/// <summary>
/// 模块中的枚举描述符
/// <para>方便使用通用接口获取枚举信息，这里的枚举只限领域Domain中的枚举</para>
/// </summary>
public class ModuleEnumDescriptor
{
    /// <summary>
    /// 枚举名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 枚举类型
    /// </summary>
    [JsonIgnore]
    public Type Type { get; set; }

    /// <summary>
    /// 枚举项下拉列表
    /// </summary>
    public IList<OptionResultModel> Options { get; set; }
}