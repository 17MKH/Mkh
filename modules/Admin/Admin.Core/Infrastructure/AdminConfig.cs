using Mkh.Config.Abstractions;

namespace Mkh.Mod.Admin.Core.Infrastructure;

/// <summary>
/// 权限管理配置信息
/// </summary>
public class AdminConfig : IConfig
{
    /// <summary>
    /// 账户默认密码(新增账户或者重置密码时使用)
    /// </summary>
    public string DefaultPassword { get; set; } = "123456";
}