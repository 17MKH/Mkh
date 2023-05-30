using Mkh.Data.Abstractions.Annotations;

namespace Mkh.Mod.Admin.Core.Domain.Account;

public partial class AccountEntity
{
    /// <summary>
    /// 租户名称
    /// </summary>
    [NotMappingColumn]
    public string TenantName { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    [NotMappingColumn]
    public string RoleName { get; set; }
}