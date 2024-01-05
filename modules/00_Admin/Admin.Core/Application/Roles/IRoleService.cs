using System;
using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Roles.Dto;
using Mkh.Mod.Admin.Core.Application.Roles.Rto;

namespace Mkh.Mod.Admin.Core.Application.Roles;

/// <summary>
/// 角色服务
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<RoleDetailsRto> Create(RoleCreateDto dto);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task Update(RoleUpdateDto dto);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<RoleDetailsRto> Get(Guid id);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(Guid id);
}