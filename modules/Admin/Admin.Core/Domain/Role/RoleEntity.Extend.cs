using Mkh.Data.Abstractions.Annotations;

namespace Mkh.Mod.Admin.Core.Domain.Role;

public partial class RoleEntity
{
    /// <summary>
    /// 菜单组名称
    /// </summary>
    [NotMappingColumn]
    public string MenuGroupName { get; set; }
}