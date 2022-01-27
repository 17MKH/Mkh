using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mkh.Mod.Admin.Core.Application.Authorize.Dto;

/// <summary>
/// 刷新令牌
/// </summary>
public class RefreshTokenDto
{
    /// <summary>
    /// 平台
    /// </summary>
    public int Platform { get; set; }

    /// <summary>
    /// 刷新令牌
    /// </summary>
    [Required]
    public string RefreshToken { get; set; }

    /// <summary>
    /// 客户IP地址
    /// </summary>
    [JsonIgnore]
    public string IP { get; set; }
}