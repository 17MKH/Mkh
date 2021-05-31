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
        /// 启用验证码功能
        /// </summary>
        public bool VerifyCode { get; set; }

        /// <summary>
        /// 启用审计日志
        /// </summary>
        public bool EnabledAuditingLog { get; set; }
    }
}
