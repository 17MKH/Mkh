using System;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Infrastructure
{
    /// <summary>
    /// 权限管理模块缓存键
    /// </summary>
    [Singleton]
    public class AdminCacheKeys
    {
        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="id">验证码ID</param>
        /// <returns></returns>
        public string VerifyCode(string id) => $"ADMIN:VERIFY_CODE:{id}";

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public string RefreshToken(Guid accountId, int platform) => $"ADMIN:REFRESH_TOKEN:{accountId}:{platform}";
    }
}
