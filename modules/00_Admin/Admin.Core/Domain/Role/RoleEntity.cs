using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.Role;

/// <summary>
/// 角色
/// </summary>
public partial class RoleEntity : EntityBaseSoftDelete
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Length(300)]
    public string Remarks { get; set; }

    /// <summary>
    /// 锁定的，不允许修改
    /// </summary>
    public bool Locked { get; set; }

    /// <summary>
    /// 绑定的菜单分组编号
    /// </summary>
    public int MenuGroupId { get; set; }
}