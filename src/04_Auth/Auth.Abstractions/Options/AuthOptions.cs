namespace Mkh.Auth.Abstractions.Options
{
    /// <summary>
    /// 认证与授权配置
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// 启用权限验证
        /// </summary>
        public bool EnablePermissionVerify { get; set; }

        /// <summary>
        /// 启用验证码功能
        /// </summary>
        public bool EnableVerifyCode { get; set; }

        /// <summary>
        /// 启用审计日志
        /// </summary>
        public bool EnableAuditingLog { get; set; }

        /// <summary>
        /// 启用检测用户IP地址
        /// </summary>
        public bool EnableCheckIP { get; set; }
    }
}
