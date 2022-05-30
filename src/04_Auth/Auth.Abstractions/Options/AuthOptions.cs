namespace Mkh.Auth.Abstractions.Options;

/// <summary>
/// 认证与授权配置
/// </summary>
public class AuthOptions
{
    /// <summary>
    /// 启用权限验证
    /// </summary>
    public bool EnablePermissionVerify { get; set; } = true;

    /// <summary>
    /// 启用验证码功能
    /// </summary>
    public bool EnableVerifyCode { get; set; } = false;

    /// <summary>
    /// 启用登录
    /// </summary>
    public bool EnableLoginLog { get; set; } = true;

    /// <summary>
    /// 启用审计日志
    /// </summary>
    public bool EnableAuditLog { get; set; } = true;

    /// <summary>
    /// 启用检测用户IP地址
    /// </summary>
    public bool EnableCheckIP { get; set; } = true;

    /// <summary>
    /// 对登录凭证进行加密
    /// </summary>
    public bool EncryptCert { get; set; } = true;
}