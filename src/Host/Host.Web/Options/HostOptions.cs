using System.Collections.Generic;

namespace Mkh.Host.Web.Options;

/// <summary>
/// 宿主配置项
/// </summary>
public class HostOptions
{
    /// <summary>
    /// 绑定的地址(默认：http://*:5000)
    /// </summary>
    public string Urls { get; set; }

    /// <summary>
    /// 基础路径
    /// </summary>
    public string Base { get; set; }

    /// <summary>
    /// 是否开启Swagger功能
    /// </summary>
    public bool Swagger { get; set; }

    /// <summary>
    /// 指定跨域访问时预检请求的有效期，单位秒，默认30分钟
    /// </summary>
    public int PreflightMaxAge { get; set; }

    /// <summary>
    /// 是否启用代理
    /// </summary>
    public bool Proxy { get; set; }

    /// <summary>
    /// 开放的wwwroot下的目录列表
    /// </summary>
    public List<string> OpenDirs { get; set; } = new() { "web" };

    /// <summary>
    /// 默认目录
    /// </summary>
    public string DefaultDir { get; set; } = "web";
}