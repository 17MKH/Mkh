using System;

namespace Mkh.Auth.Jwt
{
    /// <summary>
    /// JWT返回模型
    /// </summary>
    public class JwtResultModel
    {
        /// <summary>
        /// 账户编号
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public int Platform { get; set; }

        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
