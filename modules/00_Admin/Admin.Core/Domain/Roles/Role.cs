using Mkh.Domain.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.Roles;

/// <summary>
/// 角色
/// </summary>
internal class Role : CommonAggregateRoot
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }

    public Role()
    {
        Name = string.Empty;
        Code = string.Empty;
    }

    public Role(string name, string code)
    {
        Name = name;
        Code = code;
    }
}