// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Http;

public static class HttpContextAccessorExtensions
{
    /// <summary>
    /// 获取客户端IP地址(包含IPv和IPv6)
    /// </summary>
    /// <param name="accessor"></param>
    /// <returns></returns>
    public static string GetIP(this IHttpContextAccessor accessor)
    {
        if (accessor?.HttpContext?.Connection?.RemoteIpAddress == null)
            return "";

        return accessor.HttpContext.Connection.RemoteIpAddress.ToString();
    }

    /// <summary>
    /// 获取客户端IPv4地址
    /// </summary>
    /// <param name="accessor"></param>
    /// <returns></returns>
    public static string GetIPv4(this IHttpContextAccessor accessor)
    {
        if (accessor?.HttpContext?.Connection?.RemoteIpAddress == null)
            return "";

        return accessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }

    /// <summary>
    /// 获取客户端IPv6地址
    /// </summary>
    /// <param name="accessor"></param>
    /// <returns></returns>
    public static string GetIPv6(this IHttpContextAccessor accessor)
    {
        if (accessor?.HttpContext?.Connection?.RemoteIpAddress == null)
            return "";

        return accessor.HttpContext.Connection.RemoteIpAddress.MapToIPv6().ToString();
    }

    /// <summary>
    /// 获取客户端UserAgent
    /// </summary>
    /// <param name="accessor"></param>
    /// <returns></returns>
    public static string GetUserAgent(this IHttpContextAccessor accessor)
    {
        if (accessor?.HttpContext?.Request == null)
            return "";

        return accessor.HttpContext.Request.Headers["User-Agent"];
    }
}