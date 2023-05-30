using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mkh.Mod.Admin.Core.Application.Authorize.Dto;

/// <summary>
/// 登录
/// </summary>
public class LoginDto
{
    /// <summary>
    /// 平台
    /// </summary>
    public int Platform { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    public string VerifyCode { get; set; }

    /// <summary>
    /// 验证码ID
    /// </summary>
    public string VerifyCodeId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    [Required(ErrorMessage = "请输入用户名")]
    public string Username { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "请输入密码")]
    public string Password { get; set; }

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