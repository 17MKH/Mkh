using System;

namespace Mkh.Auth.Abstractions
{
    /// <summary>
    /// 登录结果基类
    /// </summary>
    public abstract class LoginBaseResult
    {
        /// <summary>
        /// 租户编号
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 账户编号
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 账户姓名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 应用平台
        /// <para>1-99为系统保留平台类型，用户需要自定义可使用99之后的数字</para>
        /// </summary>
        public int Platform { get; set; }

        /// <summary>
        /// 获取当前用户IP(包含IPv和IPv6)
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 获取当前用户IPv4
        /// </summary>
        public string IPv4 { get; set; }

        /// <summary>
        /// 获取当前用户IPv6
        /// </summary>
        public string IPv6 { get; set; }

        /// <summary>
        /// 登录时间戳
        /// </summary>
        public long LoginTime { get; set; }

        /// <summary>
        /// 获取UA
        /// </summary>
        public string UserAgent { get; set; }
    }
}