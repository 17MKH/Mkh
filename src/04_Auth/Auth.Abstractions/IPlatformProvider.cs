using System.Collections.Generic;

namespace Mkh.Auth.Abstractions;

/// <summary>
/// 平台提供器
/// </summary>
public interface IPlatformProvider
{
    /// <summary>
    /// 将平台值转换为说明
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    string ToDescription(int platform);

    /// <summary>
    /// 获取平台下拉列表
    /// </summary>
    /// <returns></returns>
    List<OptionResultModel> SelectOptions();
}