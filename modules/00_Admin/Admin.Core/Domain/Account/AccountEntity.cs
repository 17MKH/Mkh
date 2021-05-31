using System;
using System.ComponentModel;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.Account
{
    /// <summary>
    /// 账户信息
    /// </summary>
    [Table("Account")]
    public partial class AccountEntity : EntityBaseSoftDelete<Guid>, ITenant
    {
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户姓名或者企业名称，具体是什么与业务有关
        /// </summary>
        [Length(250)]
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Length(20)]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 邮箱
        /// </summary>
        [Length(300)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 状态
        /// </summary>
        public AccountStatus Status { get; set; }

        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime ActivateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 注销时间
        /// </summary>
        public DateTime ClosedTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 账户状态
    /// </summary>
    public enum AccountStatus
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        UnKnown = -1,
        /// <summary>
        /// 注册
        /// </summary>
        [Description("注册")]
        Register,
        /// <summary>
        /// 激活
        /// </summary>
        [Description("激活")]
        Active,
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disabled
    }
}
