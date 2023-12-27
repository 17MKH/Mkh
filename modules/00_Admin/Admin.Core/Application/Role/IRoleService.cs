using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Role.Dto;
using Mkh.Mod.Admin.Core.Application.Role.Rto;

namespace Mkh.Mod.Admin.Core.Application.Role;

/// <summary>
/// 角色服务
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// 查询所有角色信息
    /// </summary>
    /// <returns></returns>
    Task<IList<RoleDetailsRto>> QueryAll();

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<int> Add(RoleAddDto dto);

    /// <summary>
    /// 查询单个角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<RoleDetailsRto> Get(Guid id);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task Update(RoleUpdateDto dto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(Guid id);
}