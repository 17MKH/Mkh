using System;

namespace Mkh.Mod.Admin.Core.Application.Roles.Event;

/// <summary>
/// 角色删除事件
/// </summary>
public class RoleDeleteEvent
{
    public RoleDeleteEvent(Guid id, string name, string code)
    {
        Id = id;
        Name = name;
        Code = code;
        DeletedTime = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// 角色编号
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 角色编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTimeOffset DeletedTime { get; }
}