namespace Mkh.Utils.Permissions;

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
