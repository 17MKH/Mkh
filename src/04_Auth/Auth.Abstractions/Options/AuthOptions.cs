namespace Mkh.Auth.Abstractions.Options
{
    /// <summary>
    /// 认证与授权配置
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// 账户默认密码(新增账户或者重置密码时使用)
        /// </summary>
        public string DefaultPassword { get; set; } = "123456";

        /// <summary>
        /// 启用权限验证
        /// </summary>
        public bool EnablePermissionVerify { get; set; } = true;

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
