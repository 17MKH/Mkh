using System;

namespace Mkh.Mod.Admin.Core.Application.Roles.Rto;

/// <summary>
/// 角色详情
/// </summary>
public class RoleDetailsRto
{
    /// <summary>
    /// 编号
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }
}