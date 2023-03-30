using System.ComponentModel;

namespace Mkh.Auth.Abstractions;

/// <summary>
/// 权限描述符
/// </summary>
public class PermissionDescriptor
{
    /// <summary>
    /// 模块编码
    /// </summary>
    public string ModuleCode { get; set; }

    /// <summary>
    /// 控制名称
    /// </summary>
    public string Controller { get; set; }

    /// <summary>
    /// 方法名称
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    /// 请求方式
    /// </summary>
    public HttpMethod HttpMethod { get; set; }

    /// <summary>
    /// 请求方式名称
    /// </summary>
    public string HttpMethodName => HttpMethod.ToDescription();

    /// <summary>
    /// 唯一编码
    /// </summary>
    public string Code => $"{ModuleCode}_{Controller}_{Action}_{HttpMethod.ToDescription()}".ToLower();

    /// <summary>
    /// 权限模式
    /// </summary>
    public PermissionMode Mode { get; set; }
}

/// <summary>
/// 权限模式
/// </summary>
public enum PermissionMode
{
    /// <summary>
    /// 允许匿名访问
    /// </summary>
    Anonymous,
    /// <summary>
    /// 允许登录访问
    /// </summary>
    Login,
    /// <summary>
    /// 允许授权访问
    /// </summary>
    Authorization
}

/// <summary>
/// 请求方法类型
/// </summary>
public enum HttpMethod
{
    [Description("GET")]
    Get,
    [Description("PUT")]
    Put,
    [Description("POST")]
    Post,
    [Description("DELETE")]
    Delete,
    [Description("HEAD")]
    Head,
    [Description("OPTIONS")]
    Options,
    [Description("TRACE")]
    Trace,
    [Description("PATCH")]
    Patch
}