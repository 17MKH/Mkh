using System.Net.Http;

namespace Mkh.Utils.Permissions;

/// <summary>
/// 权限描述符
/// </summary>
public class PermissionDescriptor
{
    public PermissionDescriptor(string area, string controller, string action, HttpMethod httpMethod, PermissionMode mode)
    {
        Check.NotNull(area, nameof(area));
        Check.NotNull(controller, nameof(controller));
        Check.NotNull(action, nameof(action));
        Check.NotNull(httpMethod, nameof(httpMethod));

        Area = area;
        Controller = controller;
        Action = action;
        HttpMethod = httpMethod;
        Mode = mode;
    }

    /// <summary>
    /// 区域
    /// </summary>
    public string Area { get; }

    /// <summary>
    /// 控制名称
    /// </summary>
    public string Controller { get; }

    /// <summary>
    /// 方法名称
    /// </summary>
    public string Action { get; }

    /// <summary>
    /// 请求方式
    /// </summary>
    public HttpMethod HttpMethod { get; }

    /// <summary>
    /// 权限模式
    /// </summary>
    public PermissionMode Mode { get; }

    /// <summary>
    /// 唯一编码
    /// </summary>
    public string Code => $"{Area}_{Controller}_{Action}_{HttpMethod.Method}".ToLower();
}