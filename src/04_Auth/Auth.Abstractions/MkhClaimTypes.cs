namespace Mkh.Auth.Abstractions;

/// <summary>
/// 账户信息声明名称
/// </summary>
public static class MkhClaimTypes
{
    /// <summary>
    /// 租户编号
    /// </summary>
    public const string TENANT_ID = "td";

    /// <summary>
    /// 租户名称
    /// </summary>
    public const string TENANT_NAME = "tn";

    /// <summary>
    /// 账户编号
    /// </summary>
    public const string ACCOUNT_ID = "id";

    /// <summary>
    /// 账户名称
    /// </summary>
    public const string ACCOUNT_NAME = "an";

    /// <summary>
    /// 平台类型
    /// </summary>
    public const string PLATFORM = "pf";

    /// <summary>
    /// 登录时间
    /// </summary>
    public const string LOGIN_TIME = "lt";

    /// <summary>
    /// 登录IP
    /// </summary>
    public const string LOGIN_IP = "ip";
}