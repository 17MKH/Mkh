using System;

namespace Mkh.Logging.Abstractions
{
    /// <summary>
    /// 基础日志模型
    /// </summary>
    public class BaseLogModel
    {
        /// <summary>
        /// 租户编号
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 账户编号
        /// </summary>
        public Guid? AccountId { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public int Platform { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public string AccountName { get; set; }
    }
}
