namespace Mkh.Auth.Jwt
{
    /// <summary>
    /// Jwt配置项
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// 加密密钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 消费者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 令牌有效期(分钟，默认120)
        /// </summary>
        public int Expires { get; set; } = 120;

        /// <summary>
        /// 刷新令牌有效期(单位：天，默认7)
        /// </summary>
        public int RefreshTokenExpires { get; set; } = 7;
    }
}
