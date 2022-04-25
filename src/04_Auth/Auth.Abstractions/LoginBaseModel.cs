using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Mkh.Auth.Abstractions
{
    /// <summary>
    /// 登录模型
    /// </summary>
    public abstract class LoginBaseModel
    {
        /// <summary>
        /// 登录模式
        /// </summary>
        [JsonIgnore]
        public abstract LoginMode Mode { get; }

        /// <summary>
        /// 平台
        /// </summary>
        public int Platform { get; set; }

        /// <summary>
        /// 客户IP地址
        /// </summary>
        [JsonIgnore]
        public string IP { get; set; }

        /// <summary>
        /// 客户IPv4地址
        /// </summary>
        [JsonIgnore]
        public string IPv4 { get; set; }

        /// <summary>
        /// 客户IPv6地址
        /// </summary>
        [JsonIgnore]
        public string IPv6 { get; set; }

        /// <summary>
        /// 客户浏览器UA(Web端)
        /// </summary>
        [JsonIgnore]
        public string UserAgent { get; set; }

        /// <summary>
        /// 登录时间戳
        /// </summary>
        [JsonIgnore]
        public long LoginTime { get; set; }
    }

    /// <summary>
    /// 登录模式
    /// </summary>
    public enum LoginMode
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Description("用户名")]
        Username,
        /// <summary>
        /// 邮箱
        /// </summary>
        [Description("邮箱")]
        Email,
        /// <summary>
        /// 手机号
        /// </summary>
        [Description("手机号")]
        Phone,
        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        WeChat,
        /// <summary>
        /// 企业微信
        /// </summary>
        [Description("企业微信")]
        WeChatWork,
        /// <summary>
        /// QQ
        /// </summary>
        [Description("QQ")]
        QQ,
        /// <summary>
        /// 钉钉
        /// </summary>
        [Description("钉钉")]
        DingDing,
        /// <summary>
        /// 飞书
        /// </summary>
        [Description("飞书")]
        FeiShu,
        /// <summary>
        /// GitHub
        /// </summary>
        [Description("GitHub")]
        GitHub,
        /// <summary>
        /// 自定义
        /// </summary>
        [Description("自定义")]
        Custom,
    }
}
