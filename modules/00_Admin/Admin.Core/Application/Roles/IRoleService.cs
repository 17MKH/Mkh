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
    Task<Result<Guid>> CreateAsync(RoleCreateDto dto);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Result> UpdateAsync(RoleUpdateDto dto);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Result<RoleDetailsRto>> GetAsync(Guid id);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Result> DeleteAsync(Guid id);

    /// <summary>
    /// 判断指定id的角色是否都存在
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<bool> IsExistAsync(Guid[] ids);
}